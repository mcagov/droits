export const formatValidationErrors = (errorsInstance) => {
  if (errorsInstance.isEmpty()) {
    return false;
  }
  const errors = errorsInstance.array();
  const formattedErrors = {};
  errors.forEach((error) => {
    formattedErrors[error.path] = {
      id: error.path,
      href: '#' + error.path,
      value: error.value,
      text: error.msg
    };
  });
  return formattedErrors;
};
