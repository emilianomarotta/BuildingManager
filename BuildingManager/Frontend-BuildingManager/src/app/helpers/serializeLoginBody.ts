export const serializeLoginBody = (
    formatedEmail: string,
    formatedPassword: string
  ) => ({
    email: formatedEmail,
    password: formatedPassword,
  });