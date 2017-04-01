def experiment1():
    # filter accepts a function (or a lambda) which returns a boolean
    # it passes the items from the supplied list to the function and lists only those
    # items which passes lazily

    # filter returns the list lazily
    positives = filter(lambda x: x > 0, [1, -1, 2, -2, 5, -5, 12])
    print(list(positives))

    # note on how we passed None as function to filter. This would list all the values which result in True
    trues = filter(None, [0, 1, False, True, [], [1, 2], 1 == 1])
    print(list(trues))

if __name__ == '__main__':
    experiment1()