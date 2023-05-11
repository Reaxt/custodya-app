import time
import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
import math
from InterFaces.sensors import ISensor, AReading

MODEL_NAME = "PitchAndRoll Sensor"

class PitchAndRollSensor(ISensor):
    def __init__(self, gpio = 0,  model = MODEL_NAME, type = AReading.Type.PITCH_ROLL):
        self.accel_device = rt.get_acceleration_device()


    def read_sensor(self) -> list[AReading]:
        pitch_level = self.calculate_pitch()
        roll_level = self.calculate_roll()
        res = {"Pitch": pitch_level, "Roll": roll_level}
        return [AReading(AReading.Type.PITCH_ROLL, AReading.Unit.PITCH_ROLL, res)]

    def internal_get_acceleration(self):
        x = y = z = None
        for event in self.accel_device.read_loop():
                accelEvent = rt_accel.AccelerationEvent(event)
                if accelEvent.name != None:
                    if str(accelEvent.name) == "AccelerationName.X":
                        x = accelEvent.value        
                    if str(accelEvent.name) == "AccelerationName.Y":
                        y = accelEvent.value              
                    if str(accelEvent.name) == "AccelerationName.Z":
                        z = accelEvent.value
                if x and y and z:
                    return (x, y, z)
    
    def calculate_pitch(self):
        x, y, z = self.internal_get_acceleration()
        
        if x is None or y is None or z is None:
            return None
        
        pitch = math.atan2(x, math.sqrt(y ** 2 + z ** 2))
        return math.degrees(pitch)

    def calculate_roll(self):
        x, y, z = self.internal_get_acceleration()
        
        if x is None or y is None or z is None:
            return None
        
        roll = math.atan2(y, math.sqrt(x ** 2 + z ** 2))
        return math.degrees(roll)
    
if __name__ == '__main__':
    while True:
        sensor = PitchAndRollSensor()
        pitch_level = sensor.calculate_pitch()
        roll_level = sensor.calculate_roll()
        print("Pitch level:", pitch_level)
        print("Roll level:", roll_level)
        time.sleep(1)