from time import sleep 

from subsystems.security.actuators.servo import DoorController
from subsystems.security.actuators.baseactuators import ACommand

from subsystems.security.sensors.doorswitch import DoorSensor
from subsystems.security.sensors.motionsensor import MotionSensor
from subsystems.security.sensors.soundsensor import LoudnessSensor
from subsystems.security.sensors.basesensors import AReading, ISensor

DOOR_PIN = 5
MOTION_PIN = 12
LOUDNESS_BUS = 0
SERVO_PIN = 16


class SecuritySubSystem:

    def __init__(self) -> None:
        self.sensors: list[ISensor] = [
        DoorSensor(5),
        MotionSensor(MOTION_PIN),
        LoudnessSensor(LOUDNESS_BUS)
    ]
    
    def read_sensors(self):
        readings: list[AReading] = []
        for sensor in self.sensors:
            for reading in sensor.read_sensor():
                readings.append(reading)
        return readings





"""
if __name__ == "__main__":
    sensors: list[ISensor] = [
        DoorSensor(5),
        MotionSensor(MOTION_PIN),
        LoudnessSensor(LOUDNESS_BUS)
    ]
    servo = DoorController(SERVO_PIN)
    servostate = False
    oncommand = ACommand(DoorController.TARGET, '{"value":"on"}')
    offcommand = ACommand(DoorController.TARGET, '{"value":"off"}')
    while True:
        servostate = not servostate
        readings: list[AReading] = []
        for sensor in sensors:
            for reading in sensor.read_sensor():
                readings.append(reading)
        print(readings)
        if servostate:
            servo.control_actuator(oncommand.data)
        else:
            servo.control_actuator(offcommand.data)
        sleep(1)
"""