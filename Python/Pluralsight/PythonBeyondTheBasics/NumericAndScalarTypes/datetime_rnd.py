import datetime as dt


def date_experiment():
    print(dt.date.today())
    print(dt.date(1980, 12, 19))  # year, month, date
    aadhu = dt.date(year=2010, month=2, day=26)  # month -> 1 to 12 (not 0 based)
    print(aadhu.weekday())  # 0 - monday, 6 - sunday
    print(aadhu.isoweekday())  # 1 - monday, 7 - sunday
    print("Day: {}, Month: {}, Year: {} ".format(aadhu.day, aadhu.month, aadhu.year))


def date_format_experiment():
    aghil = dt.date(2014, 10, 26)
    print('use strftime  function to format dates and times')
    print('Day of the week in word ', aghil.strftime('%A'))
    print('Month name ', aghil.strftime('%B'))


def time_experiment():
    t = dt.time(23, 59, 59, 99)
    print(t.hour, t.minute, t.second, t.microsecond)
    print(t.isoformat())
    print(t.strftime("%Hh%Mm%Ss"))


def datetime_experiment():
    dt.datetime(1980, 12, 19, 10, 20, 3, 12)
    print(dt.datetime.today())
    print(dt.datetime.now())
    print(dt.datetime.utcnow())

    # today morning 8:15
    d = dt.date.today()
    t = dt.time(8, 15)

    # combine date, time
    print(dt.datetime.combine(d, t))

    # parse date
    d = dt.datetime.strptime("Monday 1 January 2016, 12:12:12",
                             "%A %d %B %Y, %M:%H:%S")
    print(d.strftime("%A"))

    # separate date&time
    print(d.date())
    print(d.time())


def timedelta_experiment():
    td = dt.timedelta(microseconds=1000, milliseconds=1)
    print(td)
    td = dt.timedelta(weeks=12, days=7, hours=12, minutes=12)
    print(td)


if __name__ == '__main__':
    timedelta_experiment()
