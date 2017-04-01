import contextlib

class Connection:
    def __init__(self):
        self.xid = 0

    def start_transaction(self):
        self.xid += 1
        print("Starting transaction {} ".format(self.xid))
        return self.xid

    def commit_transaction(self, id):
        print("Committing the transaction {}".format(id))

    def rollback_transaction(self, id):
        print("Rolling back the transaction {}".format(id))


class Transaction:
    def __init__(self, conn):
        self._conn = conn
        self.id = conn.start_transaction()

    def commit(self):
        self._conn.commit_transaction(self.id)

    def rollback(self):
        self._conn.rollback_transaction(self.id)


@contextlib.contextmanager
def start_transaction(conn):
    t = Transaction(conn)
    try:
        yield t
    except :
        t.rollback()
        raise
    t.commit()


def experiment1():
    conn = Connection()
    with start_transaction(conn) as t:
        print(t.id)

def experiment2():
    conn = Connection()
    with start_transaction(conn) as t:
        print(t.id)
        raise ValueError("Something terrible")

if __name__ == '__main__':
    experiment2()
