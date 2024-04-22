import { apiClientGET } from "@/lib/api/client/fetch";
import { NoAuthError } from "@/lib/errors";
import type { UserModel } from "@/lib/models/user/UserModel";

export const getCurrentUser = async () => {
  try {
    return await apiClientGET<UserModel>("/api/user/current");
  } catch (err) {
    if (err instanceof NoAuthError) {
      return null;
    }
    throw err;
  }
};
