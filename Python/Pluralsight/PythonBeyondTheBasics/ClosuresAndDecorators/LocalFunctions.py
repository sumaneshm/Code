
def experiment1():
    sorted = sort_by_last_letter(["Sumanesh", "Saveetha", "Aadhavan", "Aghilan"])
    for s in sorted:
        print(s)

# in this case, inner function is passed only to the sorted method and defining an function just for that purpose is
# clearly not correct and hence we declare an inner function and use it only for the sorting, it will not be visible
# outside the declared function

# inner function is bound to outer function, that is each new instance of outer function will create a new instance of
# inner function

def sort_by_last_letter(strings):
    def last_letter(s):
        return s[-1]
    return sorted(strings, key=last_letter)


def experiment2():
    # this experiment proves that the local functions are created for each instance of outer function
    outer1(1)
    outer1([2])
    outer1("sumanesh")

store = []
def outer1(s):
    def inner1():
        print("Inner1")
    print(s)
    print(inner1)
    store.append(inner1)

if __name__ == '__main__':
    experiment2()