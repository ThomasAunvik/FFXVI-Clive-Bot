import { apiGET } from "@/lib/api/fetch";
import { NoAuthError } from "@/lib/errors";
import type { UserModel } from "@/lib/models/user/UserModel";

export const getCurrentUser = async () => {
  try {
    return await apiGET<UserModel>("/api/user/current");
  } catch (err) {
    if (err instanceof NoAuthError) {
      return null;
    }
    throw err;
  }
};
