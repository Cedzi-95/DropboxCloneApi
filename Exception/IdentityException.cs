using Microsoft.AspNetCore.Identity;

public class IdentityException(string message) : Exception(message) { }

public class DateErrorException(string message) : Exception(message);