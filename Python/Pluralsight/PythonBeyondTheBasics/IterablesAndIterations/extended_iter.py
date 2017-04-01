class ExplainExtendedIter:
    def __init__(self, data_provider):
        self.data_provider = data_provider

    def __iter__(self):
        return iter(lambda: self.data_provider.next().strip(), 'END1')


class DataProvider:
    def __init__(self, data):
        self.data = data
        self.index = -1

    def next(self):
        if self.index >= len(self.data) - 1:
            raise StopIteration
        self.index += 1
        return self.data[self.index]


def experiment1():
    data = ['Line1', 'Line2', 'END', 'Line3']
    dp = DataProvider(data)
    e = ExplainExtendedIter(dp)
    i = 1
    for l in e:
        print(l)
        if i >= 5:
            break
        i += 1


if __name__ == '__main__':
    experiment1()


# iter - accepts an infinite number of list and stops when the curren value is equal to the second value
# def experiment1():
#     with open('test_data.dat', 'rt') as f:
#         for line in iter(lambda: f.readline().strip(), 'END'):
#             print(line)
#
#
# if __name__ == '__main__':
#     experiment1()

