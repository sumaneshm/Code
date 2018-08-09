import sys


def convert(s):
    """Convert the inputted string to integer
    :param s:
        an integer represented as a string
    """
    result = -1
    try:
        result = int(s)
    except ValueError:
        print("String is not an integer")
    except TypeError:
        print("s has to be an string which contains an integer")

    return result


def compact_convert(s):
    try:
        result = int(s)
    # we can catch multiple exceptions like this
    except (ValueError, TypeError):
        # if we don't want to do anything here, we can just fill up
        # the space using pass which is like noop
        pass

    return result


def print_exception_details(s):
    try:
        return int(s)
    # get the exception into a variable called 'e'
    except (ValueError, TypeError) as e:
        print("Conversion error : {}"
              .format(str(e)),
              file=sys.stderr)  # redirect the print message to standard error handler
        return -1


if __name__ == "__main__":
    print(convert('34'))  # correct conversion
    print(print_exception_details('test'))  # handled by ValueError
    print(print_exception_details([1, 2]))  # handled by TypeError
