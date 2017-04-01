class Table:
    def __init__(self, header, *data):
        self.header = header
        self.data = data
        assert len(header) == len(data)

    def _column_width(self, i):
        rslt = max(len(d) for d in self.data[i])
        return max(len(self.header[i]), rslt)

    def __str__(self):
        num_columns = len(self.header)
        column_widths = [self._column_width(i) for i in range(num_columns)]
        format_spec = ["{{:{}}}".format(column_widths[i]) for i in range(num_columns)]

        result = []

        result.append(["=" * column_widths[i] for i in range(num_columns)])
        result.append(format_spec[i].format(self.header[i]) for i in range(num_columns))
        result.append(["=" * column_widths[i] for i in range(num_columns)])

        for row in zip(*self.data):
            result.append([format_spec[i].format(row[i]) for i in range(num_columns)])

        result.append(["=" * column_widths[i] for i in range(num_columns)])

        result = ('||' + '||'.join(r) + "||" for r in result)

        return '\n'.join(result)

def experiment1():
    t = Table(['First name', 'Last name'],
              ['Sumanesh', 'Saveetha', 'Aadhavan', 'Aghilan'],
              ['Magarabooshanam', 'Chandrasekaran','Sumanesh', 'Sumanesh'])
    print(t)

if __name__ == '__main__':
    experiment1()