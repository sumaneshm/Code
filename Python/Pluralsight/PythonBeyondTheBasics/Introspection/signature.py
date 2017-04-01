from ImplementingCollections import sorted_set
import inspect


def experiment1():
    # inspect is like .net Reflection
    sign = inspect.signature(sorted_set.SortedSet.__init__)
    print(str(sign))         # prints the signature in friendly manner
    print(repr(sign))        # representation of signature for programmers

    print(sign.parameters)  # details of the parameters
    print(repr(sign.parameters['items'].default))


if __name__ == '__main__':
    experiment1()