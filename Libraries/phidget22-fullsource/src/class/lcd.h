#ifndef EXTERNALPROTO
/*
* This file is part of libphidget22
*
* Copyright 2015 Phidgets Inc <patrick@phidgets.com>
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 3 of the License, or (at your option) any later version.
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, see
* <http://www.gnu.org/licenses/>
*/
#endif

#ifndef __CPHIDGETLCD
#define __CPHIDGETLCD

typedef struct _PhidgetLCD *PhidgetLCDHandle;

API_PRETURN_HDR PhidgetLCD_create(PhidgetLCDHandle *phid);
API_PRETURN_HDR PhidgetLCD_delete(PhidgetLCDHandle *phid);

/* Properties */

API_PRETURN_HDR PhidgetLCD_getBacklight(PhidgetLCDHandle phid, double *backlight);
API_PRETURN_HDR PhidgetLCD_setBacklight(PhidgetLCDHandle phid, double backlight);

API_PRETURN_HDR PhidgetLCD_getContrast(PhidgetLCDHandle phid, double *contrast);
API_PRETURN_HDR PhidgetLCD_setContrast(PhidgetLCDHandle phid, double contrast);

API_PRETURN_HDR PhidgetLCD_setCursorBlink(PhidgetLCDHandle phid, int cursorBlink);
API_PRETURN_HDR PhidgetLCD_getCursorBlink(PhidgetLCDHandle phid, int *cursorBlink);

API_PRETURN_HDR PhidgetLCD_setCursorOn(PhidgetLCDHandle phid, int cursorOn);
API_PRETURN_HDR PhidgetLCD_getCursorOn(PhidgetLCDHandle phid, int *cursorOn);

API_PRETURN_HDR PhidgetLCD_getHeight(PhidgetLCDHandle phid, int *height);
API_PRETURN_HDR PhidgetLCD_getWidth(PhidgetLCDHandle phid, int *width);
API_PRETURN_HDR PhidgetLCD_getScreenSize(PhidgetLCDHandle phid, PhidgetLCD_ScreenSize *screenSize);
API_PRETURN_HDR PhidgetLCD_setScreenSize(PhidgetLCDHandle phid, PhidgetLCD_ScreenSize screenSize);

/* Methods */
API_PRETURN_HDR PhidgetLCD_setCharacterBitmap(PhidgetLCDHandle phid, PhidgetLCD_Font font, char character, const uint8_t *bitmap);

API_PRETURN_HDR PhidgetLCD_clear(PhidgetLCDHandle phid);

API_PRETURN_HDR PhidgetLCD_flush(PhidgetLCDHandle phid);

API_PRETURN_HDR PhidgetLCD_initialize(PhidgetLCDHandle phid);

API_PRETURN_HDR PhidgetLCD_writeText(PhidgetLCDHandle phid, PhidgetLCD_Font font, int xpos, int ypos, const char *text);

#ifdef INCLUDE_UNRELEASED

/* Unreleased API */

/* Properties */

API_PRETURN_HDR PhidgetLCD_getFrameBuffer(PhidgetLCDHandle phid, int *frameBuffer);
API_PRETURN_HDR PhidgetLCD_setFrameBuffer(PhidgetLCDHandle phid, int frameBuffer);
API_VRETURN_HDR PhidgetLCD_setFrameBuffer_async(PhidgetLCDHandle phid, int frameBuffer, Phidget_AsyncCallback fptr, void *ctx);

API_PRETURN_HDR PhidgetLCD_getSleeping(PhidgetLCDHandle phid, int *sleeping);
API_PRETURN_HDR PhidgetLCD_setSleeping(PhidgetLCDHandle phid, int sleeping);

/* Methods */

API_VRETURN_HDR PhidgetLCD_setCharacterBitmap_async(PhidgetLCDHandle phid, PhidgetLCD_Font font, char character, const uint8_t *bitmap, Phidget_AsyncCallback fptr, void *ctx);

API_VRETURN_HDR PhidgetLCD_clear_async(PhidgetLCDHandle phid, Phidget_AsyncCallback fptr, void *ctx);

API_PRETURN_HDR PhidgetLCD_copy(PhidgetLCDHandle phid, int srcFramebuffer, int dstFramebuffer, int srcX1, int srcY1, int srcX2, int srcY2, int dstX, int dstY, int inverted);
API_VRETURN_HDR PhidgetLCD_copy_async(PhidgetLCDHandle phid, int srcFramebuffer, int dstFramebuffer, int srcX1, int srcY1, int srcX2, int srcY2, int dstX, int dstY, int inverted, 
	Phidget_AsyncCallback fptr, void *ctx);

API_PRETURN_HDR PhidgetLCD_drawLine(PhidgetLCDHandle phid, int x1, int y1, int x2, int y2);
API_VRETURN_HDR PhidgetLCD_drawLine_async(PhidgetLCDHandle phid, int x1, int y1, int x2, int y2, Phidget_AsyncCallback fptr, void *ctx);

API_PRETURN_HDR PhidgetLCD_drawPixel(PhidgetLCDHandle phid, int x, int y, int state);
API_VRETURN_HDR PhidgetLCD_drawPixel_async(PhidgetLCDHandle phid, int x, int y, int state, Phidget_AsyncCallback fptr, void *ctx);

API_PRETURN_HDR PhidgetLCD_drawRect(PhidgetLCDHandle phid, int x1, int y1, int x2, int y2, int filled, int inverted);
API_VRETURN_HDR PhidgetLCD_drawRect_async(PhidgetLCDHandle phid, int x1, int y1, int x2, int y2, int filled, int inverted, Phidget_AsyncCallback fptr, void *ctx);

API_VRETURN_HDR PhidgetLCD_flush_async(PhidgetLCDHandle phid, Phidget_AsyncCallback fptr, void *ctx);

API_PRETURN_HDR PhidgetLCD_getFontSize(PhidgetLCDHandle phid, PhidgetLCD_Font font, int *width, int *height);
API_PRETURN_HDR PhidgetLCD_setFontSize(PhidgetLCDHandle phid, PhidgetLCD_Font font, int width, int height);

API_PRETURN_HDR PhidgetLCD_saveFrameBuffer(PhidgetLCDHandle phid, int frameBuffer);

API_PRETURN_HDR PhidgetLCD_getMaxCharacters(PhidgetLCDHandle phid, PhidgetLCD_Font font, int *maxCharacters);

API_PRETURN_HDR PhidgetLCD_writeBitmap(PhidgetLCDHandle phid, int xpos, int ypos, int xsize, int ysize, const uint8_t *bitmap);
API_VRETURN_HDR PhidgetLCD_writeBitmap_async(PhidgetLCDHandle phid, int xpos, int ypos, int xsize, int ysize, const uint8_t *bitmap, Phidget_AsyncCallback fptr, void *ctx);

API_VRETURN_HDR PhidgetLCD_writeText_async(PhidgetLCDHandle phid, PhidgetLCD_Font font, int xpos, int ypos, const char *text, Phidget_AsyncCallback fptr, void *ctx);

#endif

#ifndef EXTERNALPROTO

struct _PhidgetLCD {
	struct _PhidgetChannel phid;

	int width;
	int height;
	double backlight;
	double contrast;

	int cursorOn, cursorBlink;
	PhidgetLCD_ScreenSize screenSize;

#ifdef INCLUDE_UNRELEASED
	int sleep;
	int font_width[3];
	int font_height[3];
	int32_t frameBuffer;
#endif

} typedef PhidgetLCDInfo;

#endif

#endif
