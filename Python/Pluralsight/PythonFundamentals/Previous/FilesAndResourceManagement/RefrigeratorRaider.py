from contextlib import closing

class RefrigeratorRaider:

    def open(self):
        print("Open fridge door")

    def take(self, food):
        print("Finding {}...".format(food))
        if food == "fries":
            raise "Fries are not healthy..."
        print("Taking {}...".format(food))

    def close(self):
        print("Closing the fridge door")


def raid(food):
    # Experiment 1: if there is a problem in taking the food, close won't be called
    # r = RefrigeratorRaider()
    # r.open()
    # r.take(food)
    # r.close()

    # Experiment 2 : IDisposable interface
    with closing(RefrigeratorRaider()) as r:
        r.open()
        r.take(food)
        r.close()

def main():
    raid("Fruits")
    raid("fries")

if __name__ == '__main__':
    main()
