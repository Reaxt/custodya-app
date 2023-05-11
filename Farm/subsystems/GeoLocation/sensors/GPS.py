import serial
import time
import pynmea2
from InterFaces.sensors import ISensor, AReading

class GPS(ISensor):
    def __init__(self, port='/dev/ttyAMA0', baudrate=9600, type: AReading.Type = AReading.Type.GPS):
        type: AReading.Type = AReading.Type.GPS,
        self.ser = serial.Serial(port, baudrate, timeout=0.5)
        self.ser.reset_input_buffer()
        self.ser.flush()
        time.sleep(1)
    

    def read_sensor(self) -> list[AReading]:
            reading = list()
            try:
                line = self.ser.readline().decode('utf-8')
                if line.startswith('$GNGLL'):
                    data = pynmea2.parse(line)
                    lat = data.latitude
                    lng = data.longitude
                    res = {"Latitude": lat, "Longitude":lng}
                    reading.append(AReading(AReading.Type.GPS, AReading.Unit.GPS, res))
            except:
                #kevin when you see this, please make this throw an error!!
                res = {"Error":"no GPS detected"}
                return [AReading(AReading.Type.GPS, AReading.Unit.GPS, res)]
            return reading




def main():
    gps = GPS()
    while True:
        data = gps.read_sensor()
        if data is not None:
            lat, lng = data
            print("Latitude:", lat)
            print("Longitude:", lng)


if __name__ == "__main__":
    main()


 #This is a test to see if the GPS is working or not
 #   
#serial = serial.Serial('/dev/ttyAMA0', 9600, timeout=1)
#serial.reset_input_buffer()
#erial.flush()

#def print_gps_data(line):
    #print(line.rstrip())

#while True:
    #try:
        #line = serial.readline().decode('utf-8')
    #except:
        #continue
    #while len(line) > 0:
        #print_gps_data(line)
        #line = serial.readline().decode('utf-8')

    #time.sleep(1)