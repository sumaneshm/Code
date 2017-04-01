import unittest
from collections.abc import (Container, Sized, Iterable, Sequence)

from sorted_set import SortedSet

class TestConstruction(unittest.TestCase):

    def test_empty(self):
        s = SortedSet([])

    def test_from_sequence(self):
        s = SortedSet([1, 2, 3, 4])

    def test_with_duplicates(self):
        s = SortedSet([1, 1, 1])

    def test_from_iterable(self):
        def gen_iter():
            yield 1
            yield 2
            yield 3
            yield 4
        s = SortedSet(gen_iter())

    def test_default_empty(self):
        s = SortedSet()


class TestContainerProtocol(unittest.TestCase):
    def setUp(self):
        self.s = SortedSet([1, 2, 3])

    def test_positive_contained(self):
        self.assertTrue(1 in self.s)

    def test_negative_contained(self):
        self.assertFalse(4 in self.s)

    def test_positive_not_contained(self):
        self.assertTrue(4 not in self.s)

    def test_negative_not_container(self):
        self.assertFalse(1 not in self.s)

    def test_subclass_container(self):
<<<<<<< HEAD
        self.assertTrue(SortedSet, Container)
=======
        self.assertTrue(issubclass(SortedSet, Container))
>>>>>>> 777a467013647dc1c8422261bd7730ab9b2b0437


class TestSizedProtocol(unittest.TestCase):
    def test_zero_length(self):
        s = SortedSet()
        self.assertEqual(0, len(s))

    def test_zero_length_empty_list(self):
        s = SortedSet([])
        self.assertEqual(0, len(s))

    def test_eliminates_duplicates(self):
        s = SortedSet([1, 1, 2, 2])
        self.assertEqual(2, len(s))

    def test_subclass_sized(self):
<<<<<<< HEAD
        self.assertTrue(SortedSet, Sized)
=======
        self.assertTrue(issubclass(SortedSet, Sized))
>>>>>>> 777a467013647dc1c8422261bd7730ab9b2b0437


class TestIterableProtocol(unittest.TestCase):
    def setUp(self):
        self.s = SortedSet([8, 2, 4, 4, 5])

    def test_iter(self):
        it = iter(self.s)
        self.assertEqual(next(it), 2)
        self.assertEqual(next(it), 4)
        self.assertEqual(next(it), 5)
        self.assertEqual(next(it), 8)
        self.assertRaises(StopIteration, lambda : next(it))

    def test_for_loop(self):
        exp = [2, 4, 5, 8]
        for expected, actual in zip(exp, self.s):
            self.assertEqual(expected, actual)

    def test_subclass_iterable(self):
<<<<<<< HEAD
        self.assertTrue(SortedSet, Iterable)
=======
        self.assertTrue(issubclass(SortedSet, Iterable))
>>>>>>> 777a467013647dc1c8422261bd7730ab9b2b0437


class TestSequenceProtocol(unittest.TestCase):
    def setUp(self):
        self.s = SortedSet([1, 2, 3, 4, 5])

    def test_index_zero(self):
        self.assertEqual(self.s[0], 1)

    def test_index_four(self):
        self.assertEqual(self.s[4], 5)

    def test_index_one_beyond_the_end(self):
        with self.assertRaises(IndexError):
            self.s[5]

    def test_index_minus_one(self):
        self.assertEqual(self.s[-1], 5)

    def test_index_minus_five(self):
        self.assertEqual(self.s[-5], 1)

    def test_index_one_before_beginning(self):
        with self.assertRaises(IndexError):
            self.s[-6]

    def test_slice_from_start(self):
        self.assertEqual(self.s[:3], SortedSet([1, 2, 3]))

    def test_slice_from_end(self):
        self.assertEqual(self.s[3:], SortedSet([4, 5]))

    def test_slice_middle(self):
        self.assertEqual(self.s[2:3], SortedSet([3]))

    def test_slice_beyond_range(self):
        self.assertEqual(self.s[10:], SortedSet())

    # when calling reversed method, python checks whether the object implements __reversed__ method
    # if not, it will use __getitem__ and __len__
    def test_reversed(self):
        s = reversed(SortedSet([1, 2, 3]))
        i = iter(s)
        self.assertEqual(next(i), 3)
        self.assertEqual(next(i), 2)
        self.assertEqual(next(i), 1)
        with self.assertRaises(StopIteration):
            next(i)

    def test_positive_index(self):
        s = SortedSet([0, 1, 2])
        self.assertEqual(s.index(2), 2)

    def test_negative_index(self):
        s = SortedSet([0, 1, 2])
        with self.assertRaises(ValueError):
            s.index(10)

    def test_count_zero(self):
        s = SortedSet([1, 2, 3])
        self.assertEqual(0, s.count(5))

    def test_count_one(self):
        s = SortedSet([1, 2, 3])
        self.assertEqual(1, s.count(1))

    def test_subclass_sequence(self):
<<<<<<< HEAD
        self.assertTrue(SortedSet, Sequence)
=======
        self.assertTrue(issubclass(SortedSet, Sequence))

    def test_concatenate_distinct(self):
        s1 = SortedSet([1, 2, 3])
        s2 = SortedSet([4, 5, 6])
        self.assertEqual(s1 + s2, SortedSet([1, 2, 3, 4, 5, 6]))

    def test_concatenate_self(self):
        s1 = SortedSet([1, 2, 3])
        self.assertEqual(s1 + s1, s1)

    def test_concatenate_overlap(self):
        s1 = SortedSet([1, 2, 3])
        s2 = SortedSet([3, 4, 5])
        self.assertEqual(s1 + s2, SortedSet([1, 2, 3, 4, 5]))

    def test_mult_zero(self):
        s1 = SortedSet([1, 2, 3])
        self.assertEqual(s1 * 0, SortedSet())

    def test_mult_nonzero(self):
        s1 = SortedSet([1, 2, 3])
        self.assertEqual(s1 * 100, SortedSet([1, 2, 3]))

    def test_mult_right_nonzero(self):
        s1 = SortedSet([1, 2, 3])
        self.assertEqual(100 * s1, SortedSet([1, 2, 3]))
>>>>>>> 777a467013647dc1c8422261bd7730ab9b2b0437

class TestReprProtocol(unittest.TestCase):
    def test_empty_set(self):
        s = SortedSet()
        self.assertEqual(repr(s), "SortedSet()")

    def test_repr_set(self):
        s = SortedSet([1, 2, 3, 4])
        self.assertEqual(repr(s), "SortedSet([1, 2, 3, 4])")

    def test_concatenate_distinct(self):
        s1 = SortedSet([1, 2, 3])
        s2 = SortedSet([4, 5, 6])
        self.assertTrue(s1 + s2, SortedSet([1, 2, 3, 4, 5, 6]))

    def test_concatenate_self(self):
        s1 = SortedSet([1, 2, 3])
        self.assertTrue(s1 + s1, s1)

    def test_concatenate_intersection(self):
        s1 = SortedSet([1, 2, 3])
        s2 = SortedSet([3, 4, 5])
        self.assertTrue([1, 2, 3, 4, 5])

class TestEqualityProtocol(unittest.TestCase):
    def test_positive_equality(self):
        self.assertTrue(SortedSet([1, 2, 3]) == SortedSet([3, 2, 1]))

    def test_negative_equality(self):
        self.assertFalse(SortedSet([1, 2, 3]) == SortedSet([1,2]))

    def test_type_mismatch(self):
        self.assertFalse(SortedSet([1, 2, 3]) == [1, 2, 3])

    def test_identical(self):
        s = SortedSet([1, 2, 3])
        self.assertTrue(s == s)


class TestInequalityProtocol(unittest.TestCase):
    def test_positive_inequality(self):
        self.assertFalse(SortedSet([1, 2, 3]) != SortedSet([3, 2, 1]))

    def test_negative_inequality(self):
        self.assertTrue(SortedSet([1, 2, 3]) != SortedSet([1,2]))

    def test_type_mismatch(self):
        self.assertTrue(SortedSet([1, 2, 3]) != [1, 2, 3])

    def test_identical(self):
        s = SortedSet([1, 2, 3])
        self.assertFalse(s != s)


if __name__ == '__main__':
    unittest.main()
