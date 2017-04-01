__author__ = 'Sumanesh Magarabooshanam'

import random

def get_days():
    days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
    return days

def get_random_weather():
    weather = ['Sunny','Rainy','Hot','Lovely']
    report = weather[ random.randint(0, len(weather)) - 1]
    return report


def main():
    days = get_days()

    for d in days:
        r = get_random_weather()
        print("Weather on {0} is {1}".format(d,r))

print (__name__);

if __name__ == "__main__":
    main()