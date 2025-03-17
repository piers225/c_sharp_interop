#include <stdio.h>
#include <stdlib.h>
#include <arm_neon.h>

#ifdef _WIN32
#define EXPORT __declspec(dllexport)
#else
#define EXPORT
#endif

EXPORT void MultiplyMatrices(int n, double* A, double* B, double* C) {
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            C[i * n + j] = 0;
            for (int k = 0; k < n; k++) {
                C[i * n + j] += A[i * n + k] * B[k * n + j];
            }
        }
    }
}

EXPORT void MultiplyMatrices_NEON(int n, double* A, double* B, double* C) {
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            float64x2_t sum = vdupq_n_f64(0.0);  // Initialize to zero
            
            for (int k = 0; k < n; k += 2) {  // Process 2 doubles at a time
                float64x2_t a = vld1q_f64(&A[i * n + k]);
                float64x2_t b = vld1q_f64(&B[k * n + j]);
                sum = vmlaq_f64(sum, a, b);
            }

            double temp[2];
            vst1q_f64(temp, sum);
            C[i * n + j] = temp[0] + temp[1];
        }
    }
}