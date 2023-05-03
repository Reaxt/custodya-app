import serial
import time
import pynmea2
from InterFaces import ISensor, AReading

class GPS(ISensor):
    def __init__(self, port='/dev/ttyAMA0', baudrate=9600, type: AReading.Type = AReading.Type):
        type: AReading.Type = AReading.Type.GPS,
        self.ser = serial.Serial(port, baudrate, timeout=0.5)
        self.ser.reset_input_buffer()
        self.ser.flush()
        time.sleep(1)
    

    def read_sensor(self) -> list[AReading]:
        lat,lng = self.Internal_read_sensor()
        res = AReading(
            AReading.Type.GPS,
            lat,
            lng          
        )
        return [res]


    def Internal_read_sensor(self):
            try:
                line = self.ser.readline().decode('utf-8')
                if line.startswith('$GNGLL'):
                    data = pynmea2.parse(line)
                    lat = data.latitude
                    lng = data.longitude
                    return lat, lng
            except:
                pass
def main():
    GPS = Air530()
    while True:
        data = GPS.read_gps()
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