from time import sleep 

from actuators.servo import DoorController
from actuators.baseactuators import ACommand

from sensors.doorswitch import DoorSensor
from sensors.motionsensor import MotionSensor
from sensors.soundsensor import LoudnessSensor
from sensors.basesensors import AReading, ISensor

DOOR_PIN = 5
MOTION_PIN = 12
LOUDNESS_BUS = 0
SERVO_PIN = 16
if __name__ == "__main__":
    sensors: list[ISensor] = [
        DoorSensor(5),
        MotionSensor(MOTION_PIN),
        LoudnessSensor(LOUDNESS_BUS)
    ]
    servo = DoorController(SERVO_PIN)
    servostate = False;
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
