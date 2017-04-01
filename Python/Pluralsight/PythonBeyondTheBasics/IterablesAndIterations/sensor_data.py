import random
from datetime import datetime
import itertools
import time


# This class is a dummy sensor which returns infinite stream of random values
class Sensor:
    def __iter__(self):
        return self

    def __next__(self):
        return random.random()


def experiment1():
    sensor = Sensor()

    # note that this iter generates infinite number of timestamps
    timestamps = iter(datetime.now, None)

    for timestamp, sensorValue in itertools.islice(zip(timestamps, sensor), 10):
        print("{} - {}".format(timestamp, sensorValue))
        time.sleep(1)


if __name__ == '__main__':
    experiment1()
