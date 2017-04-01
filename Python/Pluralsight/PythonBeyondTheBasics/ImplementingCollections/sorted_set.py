from _bisect import bisect_left
<<<<<<< HEAD
=======
from collections import Sequence
from collections import Set
>>>>>>> 777a467013647dc1c8422261bd7730ab9b2b0437
from itertools import chain
from random import randrange
from timeit import timeit


# by inheriting Sequence, we are saying that use __getitem__ & __len__ methods to
# automatically supply "index" and "count" methods
class SortedSet(Sequence):
    def __init__(self, items=None):
        # if items is None:
        #     self._items = []
        # else:
        #     self._items = sorted(set(items))
        self._items = sorted(set(items) if not items is None else [])

    def __contains__(self, item):
        assert self._is_sorted_and_unique()
        index = bisect_left(self._items, item)
        return index < len(self._items) and self._items[index] == item

    def __len__(self):
        return len(self._items)

    def __iter__(self):
        return iter(self._items)

    def __getitem__(self, item):
        result = self._items[item]
        return SortedSet(result) if isinstance(item, slice) else result

    def __repr__(self):
        return "SortedSet({})".format(
                repr(self._items) if self._items else ''
        )

    def __eq__(self, other):
        # note how we are returning NotImplemented class instead of throwing an error,
        # this is done this way so that Python will call the equality in other way to see if that works
        if not isinstance(other, SortedSet):
            return NotImplemented

        return self._items == other._items

    def __ne__(self, other):
        # RND : python recommends to override __ne__ when overridding __eq__ for some unclear reasons
        if not isinstance(other, SortedSet):
            return NotImplemented

        return self._items != other._items

    def count_enhanced(self, item):
        return int(item in self._items)

    def count(self, item):
        assert self._is_sorted_and_unique()
        return self.count_enhanced(item)

    def index(self, item):
        assert self._is_sorted_and_unique()
        index = bisect_left(self._items, item)
        if index < len(self._items) and self._items[index] == item:
            return index
        raise ValueError("{} doesn't exist".format(item))

    def __add__(self, other):
        return SortedSet(chain(self._items, other._items))

<<<<<<< HEAD
=======
    # if our class is in the lhs, this __mul__ will be called, if it is on the rhs, __rmul__ will be called
    def __mul__(self, other):
        return self if other > 0 else SortedSet()

    def __rmul__(self, other):
        return self * other

    def _is_sorted_and_unique(self):
        assert all(self.items[i] < self.items[i+1] for i in range(len(self._items) - 1))

>>>>>>> 777a467013647dc1c8422261bd7730ab9b2b0437

def timer_experiment():
    s = SortedSet(randrange(1000) for _ in range(2000))
    c = [s.count(i) for i in range(1000)]


def experiment1():
    # SortedSet.index, count order is O(n) because it will go through the entire set to get the answer
    # but as we have specific properties to our class, i.e. it is ordered, an element can appear only once, we can
    # override both index, count to bring the order to O(logN). First we shall see the performance hit of this
    # implementation

    # this will generate 2000 random variables within the range 1 to 1000
    s = SortedSet(randrange(1000) for _ in range(2000))
    print(repr(s))
    print(len(s))

    # count the number of occurrences of each number between 1 to 1000
    # c = [s.count(i) for i in range(1000)]
    #
    # # RnD = how to achieve this using timeit?
    # # t = timeit(setup='from sorted_set import SortedSet', stmt='s = SortedSet(randrange(1000) for _ in range(2000); [s.count(i) for i in range(1000)]', number=100)
    #
    # # it will take 5 seconds for us to execute this enumeration 100 times.
    #
    # e = [s.count_enhanced(i) for i in range(1000)]
    # print(e == c)
    #
    # # this enhanced version which uses O(logN) will complete in less than 1 second
    # for i in range(100):
    #     e = [s.count_enhanced(i) for i in range(1000)]

    print("Trying to call..")
    s1 = "from __main__ import timer_experiment"
    t = timeit("timer_experiment()", setup=s1, number=100)
    print("Executed : ", t)


if __name__ == '__main__':
    experiment1()
