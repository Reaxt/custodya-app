import time
import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
import math
class VibrationSensor():
    def __init__(self):
        self.accel_device = rt.get_acceleration_device()


    def calculate_vibration_level(self):
        x1 = y1 = z1 = None
        x2 = y2 = z2 = None
        while True:
            for event in self.accel_device.read_loop():
                accelEvent = rt_accel.AccelerationEvent(event)
                if str(accelEvent.name) == "AccelerationName.X":
                    x2 = x1
                    x1 = accelEvent.value        
                if str(accelEvent.name) == "AccelerationName.Y":
                    y2 = y1
                    y1 = accelEvent.value              
                if str(accelEvent.name) == "AccelerationName.Z":
                    z2 = z1
                    z1 = accelEvent.value
                if x1 is not None and x2 is not None and y1 is not None and y2 is not None and z1 is not None and z2 is not None:
                    return math.sqrt((x1 - x2) ** 2 + (y1 - y2) ** 2 + (z1 - z2) ** 2)


if __name__ == '__main__':
    while True:
        sensor = VibrationSensor()
        vibration_level = sensor.calculate_vibration_level()
        print("Vibration level:", vibration_level)
        time.sleep(1)