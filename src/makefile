CC = gcc
CFLAGS = -O3 -fPIC
TARGET = libmatrix_operations.so
SRC = matrix_operations.c

all: $(TARGET)

$(TARGET): $(SRC)
	$(CC) -shared $(CFLAGS) -o $(TARGET) $(SRC)

clean:
	rm -f $(TARGET)
