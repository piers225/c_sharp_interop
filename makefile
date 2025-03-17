# Define the configuration
CONFIGURATION ?= Release

VECTOR3_BENCHMARK_CMD = dotnet run -c $(CONFIGURATION) --filter '*InteropBenchMark*'

.PHONY: all
all: interop-benchmark 

.PHONY: interop-benchmark
interop-benchmark:
	@echo "Running Interop..."
	$(VECTOR3_BENCHMARK_CMD)