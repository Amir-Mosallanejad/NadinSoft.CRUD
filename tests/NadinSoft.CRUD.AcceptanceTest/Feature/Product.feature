Feature: Full Product Lifecycle

    Scenario: User registers, logs in, creates, updates, deletes a product, and sees empty product list
        Given I register a new user with:
          | Email         | Password    | ConfirmPassword |
          | user@test.com | Pa$$word123 | Pa$$word123     |

        When I login using:
          | Email         | Password    |
          | user@test.com | Pa$$word123 |

        Then I should receive a JWT token

        When I use the token to create a product with:
          | Name        | ProduceDate          | ManufacturePhone | ManufactureEmail  | IsAvailable |
          | Apple Juice | 2024-01-01T00:00:00Z | +989123456789    | juice@factory.com | true        |

        Then the product should be "created" successfully

        When I retrieve the product list and extract the first product ID

        When I update the product with:
          | Name            | ProduceDate          | ManufacturePhone | ManufactureEmail  | IsAvailable |
          | Apple Juice MAX | 2024-01-01T00:00:00Z | +989123456789    | juice@factory.com | false       |

        Then the product should be "updated" successfully

        When I delete the product
        Then the product should be "deleted" successfully

        When I retrieve the product list
        Then the response should contain an empty list