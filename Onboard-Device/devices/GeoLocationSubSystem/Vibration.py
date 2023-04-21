import time
import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel

class VibrationSensor:
    def __init__(self):
        self.accel_device = rt.get_acceleration_device()

    def calculate_vibration_level(self, acceleration):
        x, y, z = acceleration
        magnitude = (x ** 2 + y ** 2 + z ** 2) ** 0.5
        return magnitude

    def run(self):
        while True:
            x = None
            y = None
            z = None
            AxisValues = [x,y,z]
            for event in self.accel_device.read_loop():
                accelEvent = rt_accel.AccelerationEvent(event)
                if accelEvent.name != None:
                    if str(accelEvent.name) == "AccelerationName.X":
                        print(accelEvent.value)
                        x = accelEvent.value
                    if str(accelEvent.name) == "AccelerationName.y":
                        print(accelEvent.value)
                        y = accelEvent.value
                    if str(accelEvent.name) == "AccelerationName.z":
                        print(accelEvent.value)
                        z = accelEvent.value
                    if x == None or  y== None or z== None:
                        continue
                    else:
                        acceleration = (x, y, z)
                        vibration_level = self.calculate_vibration_level(acceleration)
                        print(acceleration)
                        print(f"Vibration level: {vibration_level}")
                        time.sleep(0.1)
if __name__ == '__main__':
    vibration_sensor = VibrationSensor()
    vibration_sensor.run()