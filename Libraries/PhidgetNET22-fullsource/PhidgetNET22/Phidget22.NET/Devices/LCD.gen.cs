using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> LCD class definition </summary>
	public partial class LCD : Phidget {
		#region Constructor/Destructor
		/// <summary> LCD Constructor </summary>
		public LCD() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> LCD Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~LCD() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetLCD_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The backlight value </summary>
		/// <remarks>The <c>Backlight</c> affects the brightness of the LCD screen.
		/// <list>
		/// <item><c>Backlight</c> is bounded by <c>MinBackLight</c> and
		/// <c>MaxBacklight</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Backlight {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLCD_getBacklight(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_setBacklight(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The backlight value </summary>
		/// <remarks>The minimum value that <c>Backlight</c> can be set to.
		/// </remarks>
		public double MinBacklight {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLCD_getMinBacklight(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The backlight value </summary>
		/// <remarks>The maximum value that <c>Backlight</c> can be set to.
		/// </remarks>
		public double MaxBacklight {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLCD_getMaxBacklight(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The contrast value </summary>
		/// <remarks>Contrast level of the text or graphic pixels.
		/// <list>
		/// <item>A higher contrast will make the image darker.</item>
		/// <item><c>Contrast</c> is bounded by <c>MinContrast</c> and <c>MaxContrast</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Contrast {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLCD_getContrast(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_setContrast(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The contrast value. </summary>
		/// <remarks>The minimum value that <c>Contrast</c> can be set to.
		/// </remarks>
		public double MinContrast {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLCD_getMinContrast(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The contrast value. </summary>
		/// <remarks>The maximum value that <c>Contrast</c> can be set to.
		/// </remarks>
		public double MaxContrast {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLCD_getMaxContrast(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The cursor blink mode </summary>
		/// <remarks>When <c>CursorBlink</c> is true, the device will cause the cursor to periodically blink.
		/// </remarks>
		public bool CursorBlink {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetLCD_getCursorBlink(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_setCursorBlink(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The cursor on value </summary>
		/// <remarks>When <c>CursorOn</c> is true, the device will underline to the cursor position.
		/// </remarks>
		public bool CursorOn {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetLCD_getCursorOn(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_setCursorOn(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The current frame buffer </summary>
		/// <remarks>The frame buffer that is currently being used.
		/// <list>
		/// <item>Commands sent to the device are performed on this buffer.</item>
		/// </list>
		/// 
		/// </remarks>
		public int FrameBuffer {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetLCD_getFrameBuffer(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_setFrameBuffer(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The height value </summary>
		/// <remarks>The height of the LCD screen attached to the channel.
		/// </remarks>
		public int Height {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetLCD_getHeight(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The screen size </summary>
		/// <remarks>The size of the LCD screen attached to the channel.
		/// </remarks>
		public LCDScreenSize ScreenSize {
			get {
				ErrorCode result;
				LCDScreenSize val;
				result = Phidget22Imports.PhidgetLCD_getScreenSize(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_setScreenSize(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The sleep status </summary>
		/// <remarks>The on/off state of <c>Sleeping</c>. Putting the device to sleep turns off the display and
		/// backlight in order to save power.
		/// <list>
		/// <item>The device will still take commands while asleep, and will wake up if the screen is flushed, or
		/// if the contrast or backlight are changed.</item>
		/// <item>When the device wakes up, it will return to its last known state, taking into account any
		/// changes that happened while asleep.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool Sleeping {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetLCD_getSleeping(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_setSleeping(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The width value </summary>
		/// <remarks>The width of the LCD screen attached to the channel.
		/// </remarks>
		public int Width {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetLCD_getWidth(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Create a bitmap and select a character to represent it. Now, when you use the specific character,
		/// the bitmap will show in it's place.
		/// </remarks>
		public void SetCharacterBitmap(LCDFont font, string character, byte[] bitmap) {
			ErrorCode result;
			IntPtr characterPtr = UTF8Marshaler.Instance.MarshalManagedToNative(character);
			result = Phidget22Imports.PhidgetLCD_setCharacterBitmap(chandle, font, characterPtr, bitmap);
			if (result != 0) {
				Marshal.FreeHGlobal(characterPtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(characterPtr);
		}

		/// <summary> </summary>
		/// <remarks>Create a bitmap and select a character to represent it. Now, when you use the specific character,
		/// the bitmap will show in it's place.
		/// </remarks>
		public IAsyncResult BeginSetCharacterBitmap(LCDFont font, string character, byte[] bitmap, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetCharacterBitmap");
			try {
				ErrorCode result;
				IntPtr characterPtr = UTF8Marshaler.Instance.MarshalManagedToNative(character);
				result = Phidget22Imports.PhidgetLCD_setCharacterBitmap_async(chandle, font, characterPtr, bitmap, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSetCharacterBitmap</param>
		public void EndSetCharacterBitmap(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SetCharacterBitmap");
		}

		/// <summary> </summary>
		/// <remarks>The maximum number of characters that can fit on the frame buffer for the specified font.
		/// </remarks>
		public int GetMaxCharacters(LCDFont font) {
			ErrorCode result;
			int maxCharacters;
			result = Phidget22Imports.PhidgetLCD_getMaxCharacters(chandle, font, out maxCharacters);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
			return maxCharacters;
		}

		/// <summary> </summary>
		/// <remarks>Clears all pixels in the current frame buffer.
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public void Clear() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_clear(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Clears all pixels in the current frame buffer.
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginClear(AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "Clear");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_clear_async(chandle, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginClear</param>
		public void EndClear(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "Clear");
		}

		/// <summary> </summary>
		/// <remarks>Copies all pixels from a specified rectangular region to another.
		/// </remarks>
		public void Copy(int sourceFramebuffer, int destFramebuffer, int sourceX1, int sourceY1, int sourceX2, int sourceY2, int destX, int destY, bool inverted) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_copy(chandle, sourceFramebuffer, destFramebuffer, sourceX1, sourceY1, sourceX2, sourceY2, destX, destY, inverted);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Copies all pixels from a specified rectangular region to another.
		/// </remarks>
		public IAsyncResult BeginCopy(int sourceFramebuffer, int destFramebuffer, int sourceX1, int sourceY1, int sourceX2, int sourceY2, int destX, int destY, bool inverted, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "Copy");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_copy_async(chandle, sourceFramebuffer, destFramebuffer, sourceX1, sourceY1, sourceX2, sourceY2, destX, destY, inverted, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginCopy</param>
		public void EndCopy(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "Copy");
		}

		/// <summary> </summary>
		/// <remarks>Draws a straight line in the current frame buffer between two specified points
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public void DrawLine(int x1, int y1, int x2, int y2) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_drawLine(chandle, x1, y1, x2, y2);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Draws a straight line in the current frame buffer between two specified points
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginDrawLine(int x1, int y1, int x2, int y2, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "DrawLine");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_drawLine_async(chandle, x1, y1, x2, y2, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginDrawLine</param>
		public void EndDrawLine(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "DrawLine");
		}

		/// <summary> </summary>
		/// <remarks>Draws, erases, or inverts a single specified pixel.
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public void DrawPixel(int x, int y, LCDPixelState pixelState) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_drawPixel(chandle, x, y, pixelState);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Draws, erases, or inverts a single specified pixel.
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginDrawPixel(int x, int y, LCDPixelState pixelState, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "DrawPixel");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_drawPixel_async(chandle, x, y, pixelState, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginDrawPixel</param>
		public void EndDrawPixel(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "DrawPixel");
		}

		/// <summary> </summary>
		/// <remarks>Draws a rectangle in the current frame buffer using the specified points
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public void DrawRectangle(int x1, int y1, int x2, int y2, bool filled, bool inverted) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_drawRect(chandle, x1, y1, x2, y2, filled, inverted);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Draws a rectangle in the current frame buffer using the specified points
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginDrawRectangle(int x1, int y1, int x2, int y2, bool filled, bool inverted, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "DrawRectangle");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_drawRect_async(chandle, x1, y1, x2, y2, filled, inverted, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginDrawRectangle</param>
		public void EndDrawRectangle(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "DrawRectangle");
		}

		/// <summary> </summary>
		/// <remarks>Flushes the buffered LCD contents to the LCD screen.
		/// </remarks>
		public void Flush() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_flush(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Flushes the buffered LCD contents to the LCD screen.
		/// </remarks>
		public IAsyncResult BeginFlush(AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "Flush");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_flush_async(chandle, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginFlush</param>
		public void EndFlush(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "Flush");
		}

		/// <summary> </summary>
		/// <remarks>Gets the size of the specified font.
		/// </remarks>
		public LCDFontSize GetFontSize(LCDFont font) {
			ErrorCode result;
			int width;
			int height;
			result = Phidget22Imports.PhidgetLCD_getFontSize(chandle, font, out width, out height);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
			LCDFontSize ret = new LCDFontSize();
			ret.Width = width;
			ret.Height = height;
			return ret;
		}

		/// <summary> </summary>
		/// <remarks>Sets the size of the specified font.
		/// </remarks>
		public void SetFontSize(LCDFont font, int width, int height) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_setFontSize(chandle, font, width, height);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary>The current frame buffer </summary>
		/// <remarks>The frame buffer that is currently being used.
		/// <list>
		/// <item>Commands sent to the device are performed on this buffer.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSetFrameBuffer(int frameBuffer, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetFrameBuffer");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_setFrameBuffer_async(chandle, frameBuffer, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSetFrameBuffer</param>
		public void EndSetFrameBuffer(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SetFrameBuffer");
		}

		/// <summary> </summary>
		/// <remarks>Initializes the Text LCD display
		/// </remarks>
		public void Initialize() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_initialize(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Writes the specified frame buffer to flash memory
		/// <list>
		/// <item>Use sparingly. The flash memory is only designed to be written to 10,000 times before it may
		/// become unusable. This method can only be called one time each time the channel is opened.</item>
		/// </list>
		/// 
		/// </remarks>
		public void SaveFrameBuffer(int frameBuffer) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_saveFrameBuffer(chandle, frameBuffer);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Writes the specified frame buffer to flash memory
		/// <list>
		/// <item>Use sparingly. The flash memory is only designed to be written to 10,000 times before it may
		/// become unusable. This method can only be called one time each time the channel is opened.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSaveFrameBuffer(int frameBuffer, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SaveFrameBuffer");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_saveFrameBuffer_async(chandle, frameBuffer, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSaveFrameBuffer</param>
		public void EndSaveFrameBuffer(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SaveFrameBuffer");
		}

		/// <summary> </summary>
		/// <remarks>Draws a bitmap to the current frame buffer at the given location.
		/// <list>
		/// <item>Each byte in the array represents one pixel in row-major order.</item>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public void WriteBitmap(int xPosition, int yPosition, int xSize, int ySize, byte[] bitmap) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_writeBitmap(chandle, xPosition, yPosition, xSize, ySize, bitmap);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Draws a bitmap to the current frame buffer at the given location.
		/// <list>
		/// <item>Each byte in the array represents one pixel in row-major order.</item>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginWriteBitmap(int xPosition, int yPosition, int xSize, int ySize, byte[] bitmap, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "WriteBitmap");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_writeBitmap_async(chandle, xPosition, yPosition, xSize, ySize, bitmap, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginWriteBitmap</param>
		public void EndWriteBitmap(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "WriteBitmap");
		}

		/// <summary> </summary>
		/// <remarks>Writes text to the current frame buffer at the specified location
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public void WriteText(LCDFont font, int xPosition, int yPosition, string text) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLCD_writeText(chandle, font, xPosition, yPosition, text);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Writes text to the current frame buffer at the specified location
		/// <list>
		/// <item>Changes made to the frame buffer must be flushed to the LCD screen using
		/// <c>flush</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginWriteText(LCDFont font, int xPosition, int yPosition, string text, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "WriteText");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLCD_writeText_async(chandle, font, xPosition, yPosition, text, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginWriteText</param>
		public void EndWriteText(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "WriteText");
		}
		#endregion

		#region Events
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setCharacterBitmap(IntPtr phid, LCDFont font, IntPtr character, [MarshalAs(UnmanagedType.LPArray)] byte[] bitmap);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setCharacterBitmap_async(IntPtr phid, LCDFont font, IntPtr character, [MarshalAs(UnmanagedType.LPArray)] byte[] bitmap, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getMaxCharacters(IntPtr phid, LCDFont font, out int maxCharacters);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_clear(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_clear_async(IntPtr phid, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_copy(IntPtr phid, int sourceFramebuffer, int destFramebuffer, int sourceX1, int sourceY1, int sourceX2, int sourceY2, int destX, int destY, bool inverted);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_copy_async(IntPtr phid, int sourceFramebuffer, int destFramebuffer, int sourceX1, int sourceY1, int sourceX2, int sourceY2, int destX, int destY, bool inverted, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_drawLine(IntPtr phid, int x1, int y1, int x2, int y2);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_drawLine_async(IntPtr phid, int x1, int y1, int x2, int y2, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_drawPixel(IntPtr phid, int x, int y, LCDPixelState pixelState);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_drawPixel_async(IntPtr phid, int x, int y, LCDPixelState pixelState, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_drawRect(IntPtr phid, int x1, int y1, int x2, int y2, bool filled, bool inverted);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_drawRect_async(IntPtr phid, int x1, int y1, int x2, int y2, bool filled, bool inverted, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_flush(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_flush_async(IntPtr phid, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getFontSize(IntPtr phid, LCDFont font, out int width, out int height);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setFontSize(IntPtr phid, LCDFont font, int width, int height);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setFrameBuffer_async(IntPtr phid, int FrameBuffer, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_initialize(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_saveFrameBuffer(IntPtr phid, int frameBuffer);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_saveFrameBuffer_async(IntPtr phid, int frameBuffer, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_writeBitmap(IntPtr phid, int xPosition, int yPosition, int xSize, int ySize, [MarshalAs(UnmanagedType.LPArray)] byte[] bitmap);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_writeBitmap_async(IntPtr phid, int xPosition, int yPosition, int xSize, int ySize, [MarshalAs(UnmanagedType.LPArray)] byte[] bitmap, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_writeText(IntPtr phid, LCDFont font, int xPosition, int yPosition, string text);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_writeText_async(IntPtr phid, LCDFont font, int xPosition, int yPosition, string text, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getBacklight(IntPtr phid, out double Backlight);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setBacklight(IntPtr phid, double Backlight);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getMinBacklight(IntPtr phid, out double MinBacklight);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getMaxBacklight(IntPtr phid, out double MaxBacklight);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getContrast(IntPtr phid, out double Contrast);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setContrast(IntPtr phid, double Contrast);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getMinContrast(IntPtr phid, out double MinContrast);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getMaxContrast(IntPtr phid, out double MaxContrast);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getCursorBlink(IntPtr phid, out bool CursorBlink);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setCursorBlink(IntPtr phid, bool CursorBlink);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getCursorOn(IntPtr phid, out bool CursorOn);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setCursorOn(IntPtr phid, bool CursorOn);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getFrameBuffer(IntPtr phid, out int FrameBuffer);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setFrameBuffer(IntPtr phid, int FrameBuffer);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getHeight(IntPtr phid, out int Height);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getScreenSize(IntPtr phid, out LCDScreenSize ScreenSize);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setScreenSize(IntPtr phid, LCDScreenSize ScreenSize);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getSleeping(IntPtr phid, out bool Sleeping);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_setSleeping(IntPtr phid, bool Sleeping);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLCD_getWidth(IntPtr phid, out int Width);
	}
}

namespace Phidget22.Events {
}
