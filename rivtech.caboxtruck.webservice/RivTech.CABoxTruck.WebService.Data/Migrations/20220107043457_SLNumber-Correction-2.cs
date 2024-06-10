using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class SLNumberCorrection2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[SLNumber]");
            migrationBuilder.Sql(UPDATED_SL_NUMBERS);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }

        const string UPDATED_SL_NUMBERS = @"
            SET IDENTITY_INSERT [dbo].[SLNumber] ON 
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (1, 1, N'AK', N'100126117', 1, N'100137622')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (2, 1, N'AL', N'732031', 1, N'775407')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (3, 1, N'AR', N'100139303', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (4, 1, N'AZ', N'1800013867', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (5, 1, N'CA', N'N/A', 1, N'0F15123')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (6, 1, N'CO', N'482792', 1, N'523989')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (7, 1, N'CT', N'2497324', 1, N'2677364')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (8, 1, N'DC', N'3083697', 1, N'3000040633')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (9, 1, N'DE', N'1375460', 1, N'3000038484')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (10, 1, N'FL', N'N/A', 1, N'W231546')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (11, 1, N'GA', N'N/A', 1, N'3112452')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (12, 1, N'HI', N'429647', 1, N'510448')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (13, 1, N'IA', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (14, 1, N'ID', N'N/A', 1, N'783482')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (15, 1, N'IL', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (16, 1, N'IN', N'3089683', 1, N'3513192')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (17, 1, N'KS', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (18, 1, N'KY', N'894288', 1, N'936583')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (19, 1, N'LA', N'671600', 1, N'714092')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (20, 1, N'MA', N'2128952', 1, N'2115065')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (21, 1, N'MD', N'2173388', 1, N'2150537')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (22, 1, N'ME', N'N/A', 1, N'PRN278584')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (23, 1, N'MI', N'0107034', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (24, 1, N'MN', N'N/A', 1, N'40683465')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (25, 1, N'MO', N'N/A', 1, N'8392938')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (26, 1, N'MS', N'N/A', 1, N'10460145')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (27, 1, N'MT', N'100137874', 1, N'3000038477')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (28, 1, N'NC', N'1000441279', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (29, 1, N'ND', N'2000145560', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (30, 1, N'NE', N'0100235855', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (31, 1, N'NH', N'2321328', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (32, 1, N'NJ', N'1578697', 1, N'1141303')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (33, 1, N'NM', N'1800011045', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (34, 1, N'NV', N'3089444', 1, N'3543358')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (35, 1, N'NY', N'N/A', 1, N'1024521')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (36, 1, N'OH', N'1084350', 1, N'1289637')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (37, 1, N'OK', N'100229143', 1, N'100277098')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (38, 1, N'OR', N'100272853', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (39, 1, N'PA', N'758931', 1, N'940877')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (40, 1, N'RI', N'N/A', 1, N'3000038479')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (41, 1, N'SC', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (42, 1, N'SD', N'N/A', 1, N'40434090')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (43, 1, N'TN', N'N/A', 1, N'952068')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (44, 1, N'TX', N'N/A', 1, N'2514710')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (45, 1, N'UT', N'534814', 1, N'775837')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (46, 1, N'VA', N'138396', 1, N'663215')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (47, 1, N'VT', N'N/A', 1, N'3203160')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (48, 1, N'WA', N'903015', 1, N'936632')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (49, 1, N'WI', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (50, 1, N'WV', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (51, 1, N'WY', N'291414', 1, N'435743')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (52, 2, N'AK', N'N/A', 1, N'100137622')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (53, 2, N'AL', N'N/A', 1, N'775407')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (54, 2, N'AR', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (55, 2, N'AZ', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (56, 2, N'CA', N'0K57443', 1, N'0F15123')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (57, 2, N'CO', N'N/A', 1, N'523989')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (58, 2, N'CT', N'N/A', 1, N'2677364')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (59, 2, N'DC', N'N/A', 1, N'3000040633')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (60, 2, N'DE', N'N/A', 1, N'3000038484')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (61, 2, N'FL', N'N/A', 1, N'W231546')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (62, 2, N'GA', N'N/A', 1, N'3112452')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (63, 2, N'HI', N'N/A', 1, N'510448')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (64, 2, N'IA', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (65, 2, N'ID', N'N/A', 1, N'783482')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (66, 2, N'IL', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (67, 2, N'IN', N'N/A', 1, N'3513192')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (68, 2, N'KS', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (69, 2, N'KY', N'N/A', 1, N'936583')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (70, 2, N'LA', N'N/A', 1, N'714092')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (71, 2, N'MA', N'2128952', 1, N'2115065')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (72, 2, N'MD', N'N/A', 1, N'2150537')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (73, 2, N'ME', N'N/A', 1, N'PRN278584')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (74, 2, N'MI', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (75, 2, N'MN', N'N/A', 1, N'40683465')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (76, 2, N'MO', N'N/A', 1, N'8392938')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (77, 2, N'MS', N'N/A', 1, N'10460145')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (78, 2, N'MT', N'N/A', 1, N'3000038477')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (79, 2, N'NC', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (80, 2, N'ND', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (81, 2, N'NE', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (82, 2, N'NH', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (83, 2, N'NJ', N'N/A', 1, N'1141303')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (84, 2, N'NM', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (85, 2, N'NV', N'N/A', 1, N'3543358')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (86, 2, N'NY', N'1386599', 1, N'1024521')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (87, 2, N'OH', N'N/A', 1, N'1289637')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (88, 2, N'OK', N'N/A', 1, N'100277098')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (89, 2, N'OR', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (90, 2, N'PA', N'N/A', 1, N'940877')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (91, 2, N'RI', N'N/A', 1, N'3000038479')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (92, 2, N'SC', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (93, 2, N'SD', N'N/A', 1, N'40434090')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (94, 2, N'TN', N'N/A', 1, N'952068')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (95, 2, N'TX', N'2041189', 1, N'2514710')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (96, 2, N'UT', N'N/A', 1, N'775837')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (97, 2, N'VA', N'N/A', 1, N'663215')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (98, 2, N'VT', N'N/A', 1, N'3203160')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (99, 2, N'WA', N'903015', 1, N'936632')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (100, 2, N'WI', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (101, 2, N'WV', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (102, 2, N'WY', N'N/A', 1, N'435743')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (103, 3, N'AK', N'100126117', 1, N'100137622')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (104, 3, N'AL', N'732031', 1, N'775407')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (105, 3, N'AR', N'100139303', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (106, 3, N'AZ', N'1800013867', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (107, 3, N'CA', N'0K57443', 1, N'0F15123')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (108, 3, N'CO', N'482792', 1, N'523989')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (109, 3, N'CT', N'2497324', 1, N'2677364')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (110, 3, N'DC', N'3083697', 1, N'3000040633')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (111, 3, N'DE', N'1375460', 1, N'3000038484')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (112, 3, N'FL', N'N/A', 1, N'W231546')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (113, 3, N'GA', N'N/A', 1, N'3112452')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (114, 3, N'HI', N'429647', 1, N'510448')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (115, 3, N'IA', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (116, 3, N'ID', N'N/A', 1, N'783482')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (117, 3, N'IL', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (118, 3, N'IN', N'3089683', 1, N'3513192')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (119, 3, N'KS', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (120, 3, N'KY', N'894288', 1, N'936583')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (121, 3, N'LA', N'671600', 1, N'714092')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (122, 3, N'MA', N'2128952', 1, N'2115065')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (123, 3, N'MD', N'2173388', 1, N'2150537')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (124, 3, N'ME', N'N/A', 1, N'PRN278584')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (125, 3, N'MI', N'0107034', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (126, 3, N'MN', N'N/A', 1, N'40683465')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (127, 3, N'MO', N'N/A', 1, N'8392938')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (128, 3, N'MS', N'N/A', 1, N'10460145')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (129, 3, N'MT', N'100137874', 1, N'3000038477')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (130, 3, N'NC', N'1000441279', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (131, 3, N'ND', N'2000145560', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (132, 3, N'NE', N'0100235855', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (133, 3, N'NH', N'2321328', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (134, 3, N'NJ', N'1578697', 1, N'1141303')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (135, 3, N'NM', N'1800011045', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (136, 3, N'NV', N'3089444', 1, N'3543358')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (137, 3, N'NY', N'1386599', 1, N'1024521')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (138, 3, N'OH', N'1084350', 1, N'1289637')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (139, 3, N'OK', N'100229143', 1, N'100277098')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (140, 3, N'OR', N'100272853', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (141, 3, N'PA', N'758931', 1, N'940877')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (142, 3, N'RI', N'N/A', 1, N'3000038479')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (143, 3, N'SC', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (144, 3, N'SD', N'N/A', 1, N'40434090')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (145, 3, N'TN', N'N/A', 1, N'952068')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (146, 3, N'TX', N'2041189', 1, N'2514710')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (147, 3, N'UT', N'534814', 1, N'775837')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (148, 3, N'VA', N'138396', 1, N'663215')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (149, 3, N'VT', N'N/A', 1, N'3203160')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (150, 3, N'WA', N'903015', 1, N'936632')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (151, 3, N'WI', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (152, 3, N'WV', N'N/A', 1, N'8653594')
            GO
            INSERT [dbo].[SLNumber] ([Id], [CreditedOfficeId], [StateCode], [LicenseNumber], [IsActive], [ProducerSLNumber]) VALUES (153, 3, N'WY', N'291414', 1, N'435743')
            GO
            SET IDENTITY_INSERT [dbo].[SLNumber] OFF
            GO
        ";
    }
}
