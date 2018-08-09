def take(count, iterable):
    counter = 0

    for item in iterable:
        if count == counter:
            return
        counter += 1
        yield item

def distinct(iterable):
    seen = set()
    for item in iterable:
        if item in seen:
            continue
        yield item
        seen.add(item)

def run_take():
    items = ["A", "B", "C", "D"]
    for item in take(3, items):
        print(item)

def run_distinct():
    items = [1, 1, 2, 2, 1, 3, 2, 3, 4, 5, 1, 2, 5, 6]
    for item in distinct(items):
        print(item)

def run_pipeline():
    items = [1, 1, 2, 2, 1, 3, 2, 3, 4, 5, 1, 2, 5, 6]
    for item in take(3, distinct(items)):
        print(item)

if __name__ == "__main__":
    run_pipeline()
