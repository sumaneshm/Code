__author__ = 'sumaneshm'

# private variables
class NewStudent:
    __private = "Private name : Sumanesh"  # private variables preceeds with double _
    _public = 20  # Single preceeding _ or no _ means it is public
    obviously_public = True

    def PrintName(self):
        print(self.__private, " ", self._public)


ns = NewStudent()
ns.PrintName()
# print(ns.__private)                                                        #Not accessible
print(ns._public, ' ', ns.obviously_public)

