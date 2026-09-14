namespace FlowTask.Api.GraphQL.Inputs.Auth;

public record RegisterInput(
    string FullName,
    string Email,
    string Password
    //string ConfirmPassword
);