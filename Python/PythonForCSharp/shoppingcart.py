__author__ = 'sumaneshm'

class ShoppingCart:
    def __init__(self):
        self.items = []

    def addItem(self, name, price):
        item = (name, price)
        self.items.append(item)

    def __iter__(self):
        return self.items.__iter__()

    @property
    def total_price (self):
        total = 0
        for i in self.items:
            total += i[1]
        return total


cart = ShoppingCart()
cart.addItem('Car',20000)
cart.addItem('TV',40000)
cart.addItem('Laptop',3000)

for item in cart:
    print(item)

print('Total is ${0:,}'.format(cart.total_price))