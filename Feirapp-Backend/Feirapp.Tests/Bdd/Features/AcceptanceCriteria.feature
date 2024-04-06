Feature: Manage Grocery Items
Organiza Grocery Items
    Scenario: Grocery Items are created successfully
        When the client provides the right Grocery Item data
          | Name        | Price   | Cean          | Brand      | Store Name | Purchase Date | Image Url   | Category |
          | Biscoito    | 1234.56 | 7898458753257 | treloso    | Aib        | 2020-10-10    | img.url.png | 1        |
          | Bolacha     | 1234.56 | 7898458753257 | vitarella  | MIX MATEUS | 2020-10-10    | img.url.png | 1        |
          | Detergente  | 1234.56 | 7898458753257 | ype        | BOM PREÇO  | 2020-10-10    | img.url.png | 1        |
          | Cheetos Lua | 1234.56 | 7898458753257 | elma chips | VERD FRUT  | 2020-10-10    | img.url.png | 1        |
        And all of them having the following category
          | Name    | Description                        | Cest      | Item Number | Ncm   |
          | Padaria | items de cozimento e uso de massas | 123456789 | 12          | 12346 |
        Then return the Grocery Item
