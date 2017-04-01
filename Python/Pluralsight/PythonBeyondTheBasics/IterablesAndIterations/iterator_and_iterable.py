

# iterator protocol would have implemented __next__ method which would
# 1. return the next value when available
# 2. throw StopIteration once when there is no data to return
class ExampleIterator:
    def __init__(self, data):
        self.data = data
        self.index = 0

    def __next__(self):
        if self.index >= len(self.data):
            raise StopIteration
        rslt = self.data[self.index]
        self.index += 1
        return rslt


# iterable protocol should have implemented __iter__ and return an instance of iterator protocol
class ExampleIterable:
    def __init__(self, max_items):
        self.data = range(max_items)

    def __iter__(self):
        return ExampleIterator(self.data)


# ExampleIterable demonstrates the most common usecase of implementing the iterator protocol
# however there is another one way to implement iterator protocol which is by implementing __getitem__ method
# 1. __getitem__ should return the value which the parameter item point to
# 2. should throw "IndexError" if the index passed doesn't exist
class AlternateIterable:
    def __init__(self, max_items):
        self.data = range(max_items)

    def __getitem__(self, item):
        return self.data[item]


def experiment1():
    print("Conventional way using __iter__()")
    e = ExampleIterable(10)
    for i in e:
        print(i)


def experiment2():
    print("Alternate way using __getitem__()")
    e = AlternateIterable(10)
    for i in e:
        print(i)

if __name__ == '__main__':
    experiment2()