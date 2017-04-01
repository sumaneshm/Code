from fractions import Fraction
from decimal import Decimal


def fraction_experiment1():
    # There are many ways to construct a fraction
    f = Fraction(12, 2)
    print(repr(f))

    f = Fraction('22/7')
    print(repr(f))

    # though we can directly create a fraction like this, it won't be as we expect
    f = Fraction(0.1)
    print(repr(f))
    # so a more appropriate way would be like this
    f = Fraction(Decimal('0.1'))
    print(repr(f))


def complex_experiment():
    c = complex("2+3j")
    print(c)
    c = 3 + 5j
    print(c)
    print("Real : {}, Imaginary : {}".format(c.real, c.imag))


if __name__ == '__main__':
    complex_experiment()