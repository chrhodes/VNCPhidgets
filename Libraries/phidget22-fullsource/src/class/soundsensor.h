#if 0 // NOTE: left as API reference

#define SOUNDSENSOR_OCTAVES 10

API_PRETURN_HDR PhidgetSoundSensor_create(PhidgetSoundSensorHandle *phid);
API_PRETURN_HDR PhidgetSoundSensor_delete(PhidgetSoundSensorHandle *phid);

API_PRETURN_HDR PhidgetSoundSensor_getDataInterval(PhidgetSoundSensorHandle phid, uint32_t *dataInterval);
API_PRETURN_HDR PhidgetSoundSensor_setDataInterval(PhidgetSoundSensorHandle phid, uint32_t dataInterval);
API_PRETURN_HDR PhidgetSoundSensor_getMinDataInterval(PhidgetSoundSensorHandle phid, uint32_t *minDataInterval);
API_PRETURN_HDR PhidgetSoundSensor_getMaxDataInterval(PhidgetSoundSensorHandle phid, uint32_t *maxDataInterval);

API_PRETURN_HDR PhidgetSoundSensor_getdB(PhidgetSoundSensorHandle phid, double *dB);
API_PRETURN_HDR PhidgetSoundSensor_getdBA(PhidgetSoundSensorHandle phid, double *dBA);
API_PRETURN_HDR PhidgetSoundSensor_getdBC(PhidgetSoundSensorHandle phid, double *dBC);
API_PRETURN_HDR PhidgetSoundSensor_getMindB(PhidgetSoundSensorHandle phid, double *mindB);
API_PRETURN_HDR PhidgetSoundSensor_getMaxdB(PhidgetSoundSensorHandle phid, double *maxdB);

API_PRETURN_HDR PhidgetSoundSensor_getOctaves(PhidgetSoundSensorHandle phid, double (*octaves)[SOUNDSENSOR_OCTAVES]);

API_PRETURN_HDR PhidgetSoundSensor_getSPLChangeTrigger(PhidgetSoundSensorHandle phid, double *SPLChangeTrigger);
API_PRETURN_HDR PhidgetSoundSensor_setSPLChangeTrigger(PhidgetSoundSensorHandle phid, double SPLChangeTrigger);
API_PRETURN_HDR PhidgetSoundSensor_getMinSPLChangeTrigger(PhidgetSoundSensorHandle phid, double *minSPLChangeTrigger);
API_PRETURN_HDR PhidgetSoundSensor_getMaxSPLChangeTrigger(PhidgetSoundSensorHandle phid, double *maxSPLChangeTrigger);

typedef void (CCONV *PhidgetSoundSensor_OnSPLChangeCallback)(PhidgetSoundSensorHandle phid, void *ctx, double dBSPL, double dBA, double dBC, double octaves[SOUNDSENSOR_OCTAVES]);
API_PRETURN_HDR PhidgetSoundSensor_setOnSPLChangeHandler(PhidgetSoundSensorHandle phid, PhidgetSoundSensor_OnSPLChangeCallback fptr, void *ctx);


struct _PhidgetSoundSensor {
	struct _PhidgetChannel phid;

	PhidgetSoundSensor_OnSPLChangeCallback SPLChange;
	void *SPLChangeCtx;

	uint32_t dataInterval;
	uint32_t maxDataInterval;
	uint32_t minDataInterval;
	double changeTrigger;
	double maxChangeTrigger;
	double dB;
	double dBA;
	double dBC;
	double octaves[SOUNDSENSOR_OCTAVES];

	double mindB;
	double maxdB;

} typedef PhidgetSoundSensorInfo;

#endif
