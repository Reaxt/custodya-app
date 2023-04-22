import serial
import time

class Air530:
    def __init__(self, port='/dev/ttyAMA0', baudrate=9600):
        serial.reset_input_buffer()
        serial.flush()
        self.ser = serial.Serial(port, baudrate, timeout=0.5)
        time.sleep(1)

    def read_gps(self):
            try:
                # Read the data from the serial port
                data = self.ser.readline().decode('utf-8').rstrip()

                # Check if the data contains the GPS location, roll, and pitch information
                if data.startswith('$GNGGA'):
                    # Parse the GPS data
                    gps_data = data.split(',')
                    latitude = gps_data[2]
                    longitude = gps_data[4]

                elif data.startswith('$GNGSA'):
                    # Parse the roll and pitch data
                    rp_data = data.split(',')
                    roll = rp_data[3]
                    pitch = rp_data[4]
                    # Return the GPS location, roll, and pitch data
                    result = [latitude, longitude, roll, pitch]
                    return data

            except UnicodeDecodeError:
                pass

#air530 = Air530()

#while True:
    # Get the GPS location, roll, and pitch data
    #location = air530.read_gps()
    #print(location)
    # Print the data
    #print('Latitude:', location[0])
    #print('Longitude:', location[1])
    #print('Roll:', location[2])
    #print('Pitch:', location[3])
serial = serial.Serial('/dev/ttyAMA0', 9600, timeout=1)
serial.reset_input_buffer()
serial.flush()

def print_gps_data(line):
    print(line.rstrip())

while True:
    line = serial.readline().decode('utf-8')
    while len(line) > 0:
        print_gps_data(line)
        line = serial.readline().decode('utf-8')

    time.sleep(1)