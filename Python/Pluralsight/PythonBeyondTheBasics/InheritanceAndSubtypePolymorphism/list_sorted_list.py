
class SimpleList:
    def __init__(self, data):
        self._data = list(data)

    def add(self, element):
        self._data.append(element)

    def sort(self):
        self._data.sort()

    def __getitem__(self, item):
        return self._data[item]

    def __repr__(self):
        return "SimpleList : ({!r})".format(self._data)

    def __len__(self):
        return len(self._data)


class SortedList(SimpleList):
    def __init__(self, data=()):
        super().__init__(data)
        super().sort()

    def add(self, element):
        super().add(element)
        super().sort()

    def __repr__(self):
        return "SortedList : ({!r})".format(list(self))


class IntList(SimpleList):
    def __init__(self, data=()):
        for x in data:
            self.validate(x)
        super().__init__(data)

    def add(self, element):
        self.validate(element)
        super().add(element)

    @staticmethod
    def validate(i):
        if not isinstance(i, int):
            raise TypeError("IntList accepts only integers")

    def __repr__(self):
        return "IntList : ({!r})".format(list(self))


class SortedIntList(IntList, SortedList):
    def __repr__(self):
        return "SortedIntList : ({!r})".format(list(self))


def experiment1():
    s = SortedIntList((10, 2, 33, 54))
    # s.add(1)
    print(s)

if __name__ == '__main__':
    experiment1()
