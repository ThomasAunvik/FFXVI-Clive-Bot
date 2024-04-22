import { AxiosError } from "axios";
import { toast } from "sonner";

// biome-ignore lint/suspicious/noExplicitAny: <explanation>
export const getErrorInfo = (error: any): ErrorInfo => {
  if (error instanceof AxiosError) {
    let errorMessage = error.response?.data?.message;
    if (errorMessage == null) {
      errorMessage = error.message;
    }

    return {
      statusCode: error.response?.status ?? 0,
      statusMessage: error.response?.statusText ?? error.message,
      message: errorMessage,
    };
  }

  return {
    statusCode: 0,
    statusMessage: "Unknown Error",
    message: error.toString(),
  };
};

// biome-ignore lint/suspicious/noExplicitAny: <explanation>
export const toastError = (error: any) => {
  let errorMessage = "Unknown Error";
  if (error instanceof AxiosError) {
    errorMessage = error.response?.data?.message;
    if (errorMessage == null) {
      errorMessage = error.message;
    }

    const statusCode = error.response?.status ?? 0;
    errorMessage = `Request Error: (${statusCode}) ${errorMessage}`;
  }

  toast(errorMessage);
};

export interface ErrorInfo {
  statusCode: number;
  statusMessage: string;
  message?: string;
}
