__author__ = 'sumaneshm'

class ApplicationError(Exception):
    pass

def bad_method(mine):
    if mine:
        raise ApplicationError("This is my error")
    else:
        raise IndexError("Index out of bounds")


try:
    bad_method(False)
except ApplicationError as ae:
    print ("Mine... {0}".format(ae))
except Exception as e:
    print("Oops... {0}".format(e))
