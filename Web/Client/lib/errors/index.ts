class NoAuthError extends Error {
  constructor(msg: string) {
    super(msg);

    // Set the prototype explicitly.
    Object.setPrototypeOf(this, NoAuthError.prototype);
  }
}

export { NoAuthError };
