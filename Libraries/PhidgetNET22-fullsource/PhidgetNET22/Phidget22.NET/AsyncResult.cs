using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Phidget22 {
	internal class AsyncResultNoResult : IAsyncResult {
		// Fields set at construction which never change while 
		// operation is pending
		private readonly AsyncCallback m_AsyncCallback;
		private readonly Object m_AsyncState;

		// Fields set at construction which do change after 
		// operation completes
		private const Int32 c_StatePending = 0;
		private const Int32 c_StateCompletedSynchronously = 1;
		private const Int32 c_StateCompletedAsynchronously = 2;
		private Int32 m_CompletedState = c_StatePending;

		// Field that may or may not get set depending on usage
		private ManualResetEvent m_AsyncWaitHandle;

		// Fields set when operation completes
		private Exception m_exception;

		/// <summary>
		/// The object which started the operation.
		/// </summary>
		private object m_owner;

		/// <summary>
		/// Used to verify the BeginXXX and EndXXX calls match.
		/// </summary>
		private string m_operationId;

		private GCHandle unmanagedRef;
		public Phidget22Imports.AsyncCallbackEvent cCallbackDelegate;

		public AsyncResultNoResult(
			AsyncCallback asyncCallback,
			object state,
			object owner,
			string operationId) {
			m_AsyncCallback = asyncCallback;
			m_AsyncState = state;
			m_owner = owner;
			m_operationId =
				String.IsNullOrEmpty(operationId) ? String.Empty : operationId;

			//We need a delegate so the callback pointer doesn't get garbage collected
			cCallbackDelegate = new Phidget22Imports.AsyncCallbackEvent(nativeAsyncCallbackEvent);
			//This creates an unmanaged handle this object, which prevents it from being garbage collected.
			unmanagedRef = GCHandle.Alloc(this);
		}

		public bool Complete(Exception exception) {
			return this.Complete(exception, false /*completedSynchronously*/);
		}

		public bool Complete(Exception exception, bool completedSynchronously) {
			bool result = false;

			// The m_CompletedState field MUST be set prior calling the callback
			Int32 prevState = Interlocked.Exchange(ref m_CompletedState,
				completedSynchronously ? c_StateCompletedSynchronously :
				c_StateCompletedAsynchronously);
			if (prevState == c_StatePending) {
				// Passing null for exception means no error occurred. 
				// This is the common case
				m_exception = exception;

				// If the event exists, set it
				if (m_AsyncWaitHandle != null)
					m_AsyncWaitHandle.Set();

				this.MakeCallback(m_AsyncCallback, this);

				result = true;

				//Finally, allow the object to be garbage collected, as C is done with it.
				unmanagedRef.Free();
			}

			return result;
		}

		private void CheckUsage(object owner, string operationId) {
			if (!object.ReferenceEquals(owner, m_owner)) {
				throw new InvalidOperationException(
					"End was called on a different object than Begin.");
			}

			// Reuse the operation ID to detect multiple calls to end.
			if (object.ReferenceEquals(null, m_operationId)) {
				throw new InvalidOperationException(
					"End was called multiple times for this operation.");
			}

			if (!String.Equals(operationId, m_operationId)) {
				throw new ArgumentException(
					"End operation type was different than Begin.");
			}

			// Mark that End was already called.
			m_operationId = null;
		}

		public static void End(
			IAsyncResult result, object owner, string operationId) {
			AsyncResultNoResult asyncResult = result as AsyncResultNoResult;
			if (asyncResult == null) {
				throw new ArgumentException(
					"Result passed represents an operation not supported " +
					"by this framework.",
					"result");
			}

			asyncResult.CheckUsage(owner, operationId);

			// This method assumes that only 1 thread calls EndInvoke 
			// for this object
			if (!asyncResult.IsCompleted) {
				// If the operation isn't done, wait for it
				bool res = asyncResult.AsyncWaitHandle.WaitOne();
				if (!res)
					//NOTE: This should never happen - this points to a library bug, or blocking elsewhere in the program.
					asyncResult.m_exception = PhidgetException.CreateByCode(ErrorCode.Unexpected);
#if DOTNET_FRAMEWORK
				asyncResult.AsyncWaitHandle.Close();
#else
				asyncResult.AsyncWaitHandle.Dispose();
#endif
				asyncResult.m_AsyncWaitHandle = null;  // Allow early GC
			}

			// Operation is done: if an exception occurred, throw it
			if (asyncResult.m_exception != null)
				throw asyncResult.m_exception;
		}

		#region Implementation of IAsyncResult

		public Object AsyncState { get { return m_AsyncState; } }

		public bool CompletedSynchronously {
			get {
#if DOTNET_FRAMEWORK
				return Thread.VolatileRead(ref m_CompletedState) == c_StateCompletedSynchronously;
#else
				return Volatile.Read(ref m_CompletedState) == c_StateCompletedSynchronously;
#endif
			}
		}

		public WaitHandle AsyncWaitHandle {
			get {
				if (m_AsyncWaitHandle == null) {
					bool done = IsCompleted;
					ManualResetEvent mre = new ManualResetEvent(done);
					if (Interlocked.CompareExchange(ref m_AsyncWaitHandle, mre, null) != null) {
						// Another thread created this object's event; dispose 
						// the event we just created
#if DOTNET_FRAMEWORK
						mre.Close();
#else
						mre.Dispose();
#endif
					} else {
						if (!done && IsCompleted) {
							// If the operation wasn't done when we created 
							// the event but now it is done, set the event
							m_AsyncWaitHandle.Set();
						}
					}
				}
				return m_AsyncWaitHandle;
			}
		}

		public bool IsCompleted {
			get {
#if DOTNET_FRAMEWORK
				return Thread.VolatileRead(ref m_CompletedState) != c_StatePending;
#else
				return Volatile.Read(ref m_CompletedState) != c_StatePending;
#endif
			}
		}
#endregion

		protected virtual void MakeCallback(
			AsyncCallback callback, AsyncResultNoResult result) {
			// If a callback method was set, call it
			if (callback != null) {
				callback(result);
			}
		}

		private void nativeAsyncCallbackEvent(IntPtr phidgetDeviceHandle, IntPtr obj, ErrorCode returnCode) {
			Exception caughtException = null;
			if (returnCode != ErrorCode.Success)
				caughtException = PhidgetException.CreateByCode(returnCode);

			this.Complete(caughtException);
		}
	}
}
