# EvaluationTracker Frontend Development

## Packages

This application uses [React](https://react.dev) as the JavaScript framework. To
simplify the build time, plain JavaScript is used instead of TypeScript. The
following additional JavaScript packages have been installed:

* [Chakra UI](https://chakra-ui.com/) user interface components
* [Formik](https://formik.org/) for form binding
* [Lodash](https://lodash.com) general JavaScript utilities

## Notes

When building a form, use the
[`InputField`](../src/reactapp/src/components/InputField.js) tag for HTML
`<input>`, which combines Formik's `<Field>` with Chakra's `<Input>`. See
[Signup.js](../src/reactapp/src/pages/Signup.js) for example.
