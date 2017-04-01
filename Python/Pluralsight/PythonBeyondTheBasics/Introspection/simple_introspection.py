from pprint import pprint as pp
from fractions import Fraction
import sys

def experiment1():
    print(type(7))              # = int
    print(type(int))            # = type
    print(type(7)(10))          # can call the constructor directly
    i = 7
    print(i.__class__)          # = int
    print(i.__class__.__class__)    # = type

    # type and object are interdependent and they are fundamental to python
    print(type(object))
    print(issubclass(type, object))

    print(isinstance(i, int))


# introspecting objects
def experiment2():
    i = 10
    pp(dir(i))                          # shows all the attributes, methods this object has
    print(getattr(i, "denominator"))    # attributes can be directly called like this
    print(hasattr(i, "numerator"))      # check for a presence of an attribute can be done like this


# Python PEP - Easier to ask for forgiveness rather than asking for permissions
def mix_numeral(vulgar):
    try:
        integer = vulgar.numerator // vulgar.denominator
        frac = Fraction(vulgar.numerator - integer * vulgar.denominator, vulgar.denominator)
        return integer, frac
    except AttributeError as e:
        raise ValueError("{} is not a proper rational number".format(vulgar)) from e


def experiment3():
    print(mix_numeral(Fraction('11/10')))

    try:
        print(mix_numeral(1.1))
    except ValueError as e:
       print(sys.exc_info())


if __name__ == '__main__':
    experiment3()