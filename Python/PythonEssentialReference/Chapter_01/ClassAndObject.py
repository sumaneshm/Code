class Stack(object):
    def __init__(self):
        self.stack = []

    def push(self, object):
        self.stack.append(object)

    def pop(self):
        return self.stack.pop()

    def len(self):
        return len(self.stack)

s = Stack()
s.push("Sumanesh")
s.push(35)
s.push([12, 13])

while s.len() > 0:
    print(s.pop())

del s
