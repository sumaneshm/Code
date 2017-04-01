import math


def experiment1():
    items = {
        'number': 42,
        'text': 'Hello world',
        10: 'Ten'
    }

    print(items['number'])
    print(items[10])

    items["function"] = abs         # We can add functions as items
    print(items["function"](-10))

    items["module"] = math          # modules can also be added
    print(items["module"].sqrt(4))

    items["Error"] = ValueError     # Exception can also be added
    try:
        int("something wrong")
    except items["Error"] as e:
        print("Caught {}, Details {}".format(items["Error"], e))


def experiment2():
    l = "'GOOG', 10, 490.10"
    fs = [str, int, float]

    raw_data = l.split(',')

    # observe how we are using the function, data to convert the data
    data = [f(d) for f, d in zip(fs, raw_data)]
    print(data)


def experiment3():
    s = "Sumanesh"
    print(s.center(30))


if __name__ == '__main__':
    experiment3()
