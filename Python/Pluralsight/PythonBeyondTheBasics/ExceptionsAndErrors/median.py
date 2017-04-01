def median(items):
    s = sorted(items)

    if len(s) == 0:
        raise ValueError("empty list of values are not expected")

    median_index = (len(s) - 1) // 2
    if len(s) % 2 != 0:
        return s[median_index]

    return (s[median_index] + s[median_index + 1]) / 2.0


def experiment1():
    print(median([3, 1, 2, 5, 4]))
    print(median([1, 2, 3, 4, 5, 6]))

    try:
        median([])
    except ValueError as e:
        print("Error : ", e.args)

if __name__ == '__main__':
    experiment1()

