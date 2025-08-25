using LibraryManagement.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Books endpoints
app.MapGet("/api/books", () =>
{
    return new List<Book>
    {
        new Book
        {
            Id = 1,
            Title = "The Great Gatsby",
            Isbn = "978-0-7432-7356-5",
            PublishedDate = new DateOnly(1925, 4, 10),
            Genre = "Fiction",
            Pages = 180,
            Available = true,
            Authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "F. Scott", LastName = "Fitzgerald", BirthDate = new DateOnly(1896, 9, 24) }
            },
            Branch = new LibraryBranch { Id = 1, Name = "Main Library", Address = "123 Main St", Phone = "555-0123" }
        }
    };
});

app.MapGet("/api/books/{id}", (int id) =>
{
    return new Book
    {
        Id = id,
        Title = "To Kill a Mockingbird",
        Isbn = "978-0-06-112008-4",
        PublishedDate = new DateOnly(1960, 7, 11),
        Genre = "Fiction",
        Pages = 324,
        Available = false,
        Authors = new List<Author>
        {
            new Author { Id = 2, FirstName = "Harper", LastName = "Lee", BirthDate = new DateOnly(1926, 4, 28) }
        },
        Branch = new LibraryBranch { Id = 1, Name = "Main Library", Address = "123 Main St", Phone = "555-0123" },
        CurrentLoan = new BookLoan
        {
            Id = 1,
            BookId = id,
            MemberId = 1,
            LoanDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            Member = new Member { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", MembershipDate = new DateOnly(2020, 1, 15) }
        }
    };
});

app.MapPost("/api/books", (CreateBookRequest request) =>
{
    return new Book
    {
        Id = 100,
        Title = request.Title,
        Isbn = request.Isbn,
        PublishedDate = request.PublishedDate,
        Genre = request.Genre,
        Pages = request.Pages,
        Available = true,
        Authors = request.AuthorIds.Select(id => new Author { Id = id, FirstName = "New", LastName = "Author" }).ToList(),
        Branch = new LibraryBranch { Id = request.BranchId, Name = "Branch Library" }
    };
});

// Authors endpoints
app.MapGet("/api/authors", () =>
{
    return new List<AuthorWithBooks>
    {
        new AuthorWithBooks
        {
            Id = 1,
            FirstName = "Agatha",
            LastName = "Christie",
            BirthDate = new DateOnly(1890, 9, 15),
            Biography = "British crime novelist and playwright",
            Books = new List<BookSummary>
            {
                new BookSummary { Id = 10, Title = "Murder on the Orient Express", Genre = "Mystery" },
                new BookSummary { Id = 11, Title = "And Then There Were None", Genre = "Mystery" }
            },
            Statistics = new AuthorStatistics
            {
                TotalBooks = 2,
                TotalLoans = 150,
                AverageRating = 4.5,
                MostPopularGenre = "Mystery"
            }
        }
    };
});

// Members endpoints
app.MapGet("/api/members/{id}/loans", (int id) =>
{
    return new MemberLoanHistory
    {
        Member = new Member
        {
            Id = id,
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice.johnson@email.com",
            Phone = "555-0456",
            MembershipDate = new DateOnly(2019, 3, 20),
            MembershipType = "Premium"
        },
        ActiveLoans = new List<BookLoan>
        {
            new BookLoan
            {
                Id = 1,
                BookId = 5,
                LoanDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(9)),
                Book = new BookSummary { Id = 5, Title = "1984", Genre = "Dystopian Fiction" }
            }
        },
        LoanHistory = new List<CompletedLoan>
        {
            new CompletedLoan
            {
                BookTitle = "Pride and Prejudice",
                LoanDate = new DateOnly(2024, 1, 15),
                ReturnDate = new DateOnly(2024, 2, 1),
                Rating = 5
            }
        },
        Statistics = new MemberStatistics
        {
            TotalBooksLoaned = 25,
            BooksCurrentlyLoaned = 1,
            AverageRating = 4.2,
            FavoriteGenre = "Fiction",
            LateFees = 0.0m
        }
    };
});

// Library branches endpoints
app.MapGet("/api/branches/{id}/inventory", (int id) =>
{
    return new BranchInventory
    {
        Branch = new LibraryBranch
        {
            Id = id,
            Name = "Downtown Branch",
            Address = "456 Oak Street",
            Phone = "555-0789",
            OpeningHours = "Mon-Fri: 9AM-8PM, Sat-Sun: 10AM-6PM"
        },
        TotalBooks = 15420,
        AvailableBooks = 12380,
        BooksByGenre = new Dictionary<string, int>
        {
            ["Fiction"] = 5200,
            ["Non-Fiction"] = 3800,
            ["Mystery"] = 2100,
            ["Science Fiction"] = 1850,
            ["Romance"] = 1200,
            ["Biography"] = 1270
        },
        PopularBooks = new List<BookSummary>
        {
            new BookSummary { Id = 1, Title = "The Seven Husbands of Evelyn Hugo", Genre = "Fiction" },
            new BookSummary { Id = 2, Title = "Atomic Habits", Genre = "Self-Help" }
        },
        RecentActivity = new BranchActivity
        {
            LoansToday = 47,
            ReturnsToday = 52,
            NewMembersThisWeek = 8,
            OverdueBooks = 23
        }
    };
});

// Advanced Analytics endpoints - Complex nested objects 3+ levels deep
app.MapGet("/api/analytics/system/comprehensive", () =>
{
    return new ComprehensiveLibraryAnalytics
    {
        LibrarySystemId = Guid.NewGuid(),
        GeneratedAt = DateTime.UtcNow,
        SystemMetrics = new SystemWideMetrics
        {
            Circulation = new CirculationMetrics
            {
                TotalData = new TotalCirculationData
                {
                    TotalLoans = 15420,
                    TotalReturns = 14890,
                    RecentLoans = new List<LoanDetail>
                    {
                        new LoanDetail
                        {
                            Book = new BookSummary { Id = 1, Title = "Advanced Database Systems", Genre = "Technology" },
                            Member = new Member { Id = 1, FirstName = "Sarah", LastName = "Chen", Email = "sarah.chen@email.com", MembershipDate = new DateOnly(2020, 3, 15) },
                            LoanDate = DateTime.UtcNow.AddDays(-5),
                            DueDate = DateTime.UtcNow.AddDays(9),
                            Events = new List<LoanEvent>
                            {
                                new LoanEvent
                                {
                                    EventDate = DateTime.UtcNow.AddDays(-5),
                                    EventType = "Loan",
                                    Details = new List<EventDetail>
                                    {
                                        new EventDetail
                                        {
                                            DetailType = "Channel",
                                            Value = "Online Portal",
                                            Notes = new List<string> { "Self-service loan", "Digital verification" }
                                        }
                                    },
                                    ProcessedBy = new StaffMember
                                    {
                                        EmployeeId = "EMP001",
                                        Name = "System Automated",
                                        Role = "Digital Services",
                                        Permissions = new List<string> { "Auto-loan", "Digital-verification" }
                                    }
                                }
                            },
                            Context = new LoanContext
                            {
                                Channel = "Online Portal",
                                Branch = new LibraryBranch { Id = 1, Name = "Main Library", Address = "123 Main St" },
                                Flags = new List<SystemFlag>
                                {
                                    new SystemFlag
                                    {
                                        FlagType = "High Demand Item",
                                        Reason = "Popular category with wait list",
                                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                                        Implications = new List<string> { "Monitor return date", "Priority processing" }
                                    }
                                },
                                Metadata = new TransactionMetadata
                                {
                                    TransactionId = "TXN-2024-001234",
                                    SystemData = new Dictionary<string, object>
                                    {
                                        ["ProcessingTime"] = 1.2,
                                        ["ValidationMethod"] = "Digital",
                                        ["QueuePosition"] = 0
                                    },
                                    AuditTrail = new List<AuditEntry>
                                    {
                                        new AuditEntry
                                        {
                                            Timestamp = DateTime.UtcNow.AddDays(-5),
                                            Action = "LOAN_CREATED",
                                            UserId = "sarah.chen@email.com",
                                            Changes = new List<FieldChange>
                                            {
                                                new FieldChange
                                                {
                                                    FieldName = "Status",
                                                    OldValue = "Available",
                                                    NewValue = "On Loan",
                                                    Validators = new List<string> { "InventoryValidator", "MemberEligibilityValidator" }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Renewals = new RenewalStatistics
                    {
                        TotalRenewals = 3240,
                        Patterns = new List<RenewalPattern>
                        {
                            new RenewalPattern
                            {
                                PatternType = "Category-based",
                                Metrics = new Dictionary<string, double>
                                {
                                    ["Fiction"] = 0.68,
                                    ["Technical"] = 0.82,
                                    ["Biography"] = 0.45
                                },
                                Observations = new List<PatternObservation>
                                {
                                    new PatternObservation
                                    {
                                        ObservationType = "Usage Behavior",
                                        Description = "Technical books have higher renewal rates",
                                        Evidence = new List<SupportingData>
                                        {
                                            new SupportingData
                                            {
                                                DataType = "Statistical",
                                                Values = new Dictionary<string, object>
                                                {
                                                    ["ConfidenceInterval"] = 0.95,
                                                    ["SampleSize"] = 1250,
                                                    ["SignificanceLevel"] = 0.001
                                                },
                                                Sources = new List<string> { "Circulation Database", "Member Surveys" }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Analysis = new List<RenewalAnalysis>
                        {
                            new RenewalAnalysis
                            {
                                AnalysisType = "Predictive",
                                Score = 0.78,
                                Factors = new List<RenewalFactor>
                                {
                                    new RenewalFactor
                                    {
                                        FactorName = "Member Engagement",
                                        Weight = 0.35,
                                        Components = new List<FactorComponent>
                                        {
                                            new FactorComponent
                                            {
                                                ComponentName = "Historical Usage",
                                                Value = 0.82,
                                                Influences = new List<string> { "Previous renewals", "Category preferences", "Seasonal patterns" }
                                            }
                                        }
                                    }
                                },
                                Predictions = new PredictiveModel
                                {
                                    ModelType = "Random Forest",
                                    Accuracy = 0.87,
                                    Predictions = new List<ModelPrediction>
                                    {
                                        new ModelPrediction
                                        {
                                            PredictionDate = DateTime.UtcNow.AddDays(30),
                                            PredictedValue = 3450.0,
                                            Confidence = 0.89,
                                            Variables = new List<PredictionVariable>
                                            {
                                                new PredictionVariable
                                                {
                                                    VariableName = "SeasonalFactor",
                                                    Coefficient = 0.23,
                                                    Assumptions = new List<string> { "No major holidays", "Normal weather patterns" }
                                                }
                                            }
                                        }
                                    },
                                    Validation = new ModelValidation
                                    {
                                        Metrics = new List<ValidationMetric>
                                        {
                                            new ValidationMetric
                                            {
                                                MetricName = "Mean Absolute Error",
                                                Value = 0.12,
                                                Interpretation = "Excellent predictive accuracy",
                                                Benchmarks = new List<string> { "Industry Standard: 0.15", "Historical Best: 0.11" }
                                            }
                                        },
                                        Results = new ValidationResults
                                        {
                                            IsValid = true,
                                            OverallScore = 0.94,
                                            Components = new List<ResultComponent>
                                            {
                                                new ResultComponent
                                                {
                                                    ComponentName = "Cross-Validation",
                                                    Passed = true,
                                                    Details = new List<string> { "5-fold CV passed", "Low variance detected" }
                                                }
                                            }
                                        },
                                        Issues = new List<ValidationIssue>
                                        {
                                            new ValidationIssue
                                            {
                                                IssueType = "Data Quality",
                                                Severity = "Low",
                                                Description = "Minor gaps in historical data for December",
                                                Resolution = new List<RemediationAction>
                                                {
                                                    new RemediationAction
                                                    {
                                                        ActionType = "Data Imputation",
                                                        Description = "Fill gaps using seasonal interpolation",
                                                        Priority = 2,
                                                        Steps = new List<string> { "Identify gap patterns", "Apply seasonal adjustment", "Validate results" }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Trends = new List<CirculationTrend>
                {
                    new CirculationTrend
                    {
                        TrendName = "Digital vs Physical Loans",
                        DataPoints = new List<TrendDataPoint>
                        {
                            new TrendDataPoint
                            {
                                Date = DateTime.UtcNow.AddDays(-30),
                                Value = 0.45, // Digital ratio
                                Context = new List<DataPointContext>
                                {
                                    new DataPointContext
                                    {
                                        ContextType = "External Event",
                                        Data = new Dictionary<string, object>
                                        {
                                            ["Event"] = "System Upgrade",
                                            ["Impact"] = "Positive",
                                            ["Duration"] = "2 days"
                                        },
                                        Tags = new List<string> { "technology", "upgrade", "digital-preference" }
                                    }
                                }
                            }
                        },
                        Analysis = new TrendAnalysis
                        {
                            Direction = "Increasing",
                            Velocity = 0.03, // 3% per month
                            Insights = new List<TrendInsight>
                            {
                                new TrendInsight
                                {
                                    InsightType = "Technology Adoption",
                                    Message = "Digital lending preference accelerating among younger demographics",
                                    Confidence = 0.91,
                                    Evidence = new List<InsightEvidence>
                                    {
                                        new InsightEvidence
                                        {
                                            EvidenceType = "Demographic Data",
                                            Data = new Dictionary<string, object>
                                            {
                                                ["AgeGroup18-35"] = 0.78,
                                                ["AgeGroup36-50"] = 0.52,
                                                ["AgeGroup50+"] = 0.23
                                            },
                                            Sources = new List<string> { "Member Profile Database", "Usage Analytics" }
                                        }
                                    }
                                }
                            },
                            Statistics = new StatisticalSummary
                            {
                                Mean = 0.48,
                                Median = 0.47,
                                StandardDeviation = 0.08,
                                Outliers = new List<OutlierAnalysis>
                                {
                                    new OutlierAnalysis
                                    {
                                        Date = DateTime.UtcNow.AddDays(-15),
                                        Value = 0.72,
                                        Classification = "Positive Outlier",
                                        PossibleCauses = new List<OutlierCause>
                                        {
                                            new OutlierCause
                                            {
                                                CauseType = "Promotional Event",
                                                Likelihood = 0.85,
                                                Indicators = new List<string> { "Digital literacy workshop", "New user registrations spike" }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Forecasts = new List<TrendForecast>
                        {
                            new TrendForecast
                            {
                                ForecastDate = DateTime.UtcNow.AddMonths(6),
                                ForecastValue = 0.62,
                                ConfidenceInterval = 0.85,
                                Assumptions = new List<ForecastAssumption>
                                {
                                    new ForecastAssumption
                                    {
                                        AssumptionType = "Technology Infrastructure",
                                        Description = "Current digital platform capacity maintained",
                                        RiskFactors = new List<string> { "Server capacity limits", "Budget constraints for upgrades" }
                                    }
                                }
                            }
                        }
                    }
                },
                PopularityByCategory = new Dictionary<string, PopularityMetric>
                {
                    ["Technology"] = new PopularityMetric
                    {
                        Category = "Technology",
                        TotalLoans = 2340,
                        TopItems = new List<PopularItem>
                        {
                            new PopularItem
                            {
                                Book = new BookSummary { Id = 101, Title = "Clean Code", Genre = "Programming" },
                                LoanCount = 89,
                                Factors = new List<PopularityFactor>
                                {
                                    new PopularityFactor
                                    {
                                        FactorType = "Professional Development",
                                        Impact = 0.72,
                                        Attributes = new List<string> { "Career advancement", "Skill building", "Industry standard" }
                                    }
                                },
                                Demographics = new DemographicBreakdown
                                {
                                    AgeGroups = new Dictionary<string, int>
                                    {
                                        ["25-35"] = 45,
                                        ["36-45"] = 32,
                                        ["46-55"] = 12
                                    },
                                    Insights = new List<DemographicInsight>
                                    {
                                        new DemographicInsight
                                        {
                                            InsightType = "Career Stage Correlation",
                                            Description = "Highest demand among mid-career professionals",
                                            Implications = new List<string> { "Target professional development programs", "Consider workplace partnerships" }
                                        }
                                    }
                                }
                            }
                        },
                        Trends = new PopularityTrends
                        {
                            Periods = new List<TrendPeriod>
                            {
                                new TrendPeriod
                                {
                                    PeriodName = "Q1 2024",
                                    StartDate = DateTime.Parse("2024-01-01"),
                                    EndDate = DateTime.Parse("2024-03-31"),
                                    Metrics = new List<PeriodMetric>
                                    {
                                        new PeriodMetric
                                        {
                                            MetricName = "Growth Rate",
                                            Value = 0.15,
                                            Influences = new List<string> { "New technology releases", "Industry conferences" }
                                        }
                                    }
                                }
                            },
                            Seasonality = new SeasonalityAnalysis
                            {
                                Patterns = new List<SeasonalPattern>
                                {
                                    new SeasonalPattern
                                    {
                                        PatternName = "Back to School Surge",
                                        Season = "Fall",
                                        Intensity = 0.68,
                                        Drivers = new List<PatternDriver>
                                        {
                                            new PatternDriver
                                            {
                                                DriverType = "Academic Calendar",
                                                Description = "Students and professionals upgrading skills",
                                                Triggers = new List<string> { "Semester start", "New course requirements" }
                                            }
                                        }
                                    }
                                },
                                Forecast = new SeasonalForecast
                                {
                                    Predictions = new List<SeasonalPrediction>
                                    {
                                        new SeasonalPrediction
                                        {
                                            Season = "Summer 2024",
                                            PredictedChange = -0.12,
                                            KeyCategories = new List<string> { "Academic Textbooks", "Professional Certification" }
                                        }
                                    },
                                    Accuracy = new ForecastAccuracy
                                    {
                                        HistoricalAccuracy = 0.83,
                                        Metrics = new List<AccuracyMetric>
                                        {
                                            new AccuracyMetric
                                            {
                                                MetricName = "Seasonal MAPE",
                                                Value = 0.11,
                                                Limitations = new List<string> { "Extreme weather events not accounted", "Major technology disruptions" }
                                            }
                                        }
                                    }
                                }
                            },
                            EmergingTrends = new List<EmergingTrend>
                            {
                                new EmergingTrend
                                {
                                    TrendName = "AI/Machine Learning Resources",
                                    DetectedDate = DateTime.UtcNow.AddDays(-45),
                                    GrowthRate = 0.35,
                                    Indicators = new List<TrendIndicator>
                                    {
                                        new TrendIndicator
                                        {
                                            IndicatorName = "Search Query Frequency",
                                            Strength = 0.87,
                                            Manifestations = new List<string> { "Increased catalog searches", "Wait list formations", "Inter-library requests" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                SeasonalPatterns = new SeasonalAnalysis
                {
                    Trends = new List<SeasonalTrend>
                    {
                        new SeasonalTrend
                        {
                            Season = "Winter",
                            Metrics = new List<TrendMetric>
                            {
                                new TrendMetric
                                {
                                    MetricName = "Fiction Preference",
                                    Value = 0.78,
                                    YearOverYearChange = 0.05,
                                    Factors = new List<string> { "Holiday reading", "Indoor activities", "Gift giving" }
                                }
                            },
                            Context = new SeasonalContext
                            {
                                Factors = new List<ContextualFactor>
                                {
                                    new ContextualFactor
                                    {
                                        FactorType = "Weather",
                                        Description = "Colder temperatures drive indoor activities",
                                        Impact = 0.65,
                                        Manifestations = new List<string> { "Increased library visits", "Longer browsing sessions" }
                                    }
                                },
                                ExternalInfluences = new Dictionary<string, object>
                                {
                                    ["HolidaySchedule"] = "Extended hours during holidays",
                                    ["CommunityEvents"] = "Reading clubs, book discussions"
                                }
                            }
                        }
                    },
                    Seasons = new Dictionary<string, SeasonalMetrics>
                    {
                        ["Spring"] = new SeasonalMetrics
                        {
                            AverageActivity = 1.15,
                            Categories = new List<CategoryPerformance>
                            {
                                new CategoryPerformance
                                {
                                    Category = "Gardening",
                                    PerformanceIndex = 1.85,
                                    Drivers = new List<PerformanceDriver>
                                    {
                                        new PerformanceDriver
                                        {
                                            DriverName = "Seasonal Interest",
                                            Contribution = 0.75,
                                            Mechanisms = new List<string> { "Planting season", "Home improvement projects" }
                                        }
                                    }
                                }
                            },
                            Variation = new VariationAnalysis
                            {
                                Coefficient = 0.18,
                                Sources = new List<VariationSource>
                                {
                                    new VariationSource
                                    {
                                        SourceName = "Weather Variability",
                                        Variance = 0.12,
                                        Explanations = new List<string> { "Early vs late spring", "Unexpected weather patterns" }
                                    }
                                }
                            }
                        }
                    },
                    Recommendations = new List<SeasonalRecommendation>
                    {
                        new SeasonalRecommendation
                        {
                            Season = "Fall",
                            RecommendationType = "Collection Enhancement",
                            Insights = new List<ActionableInsight>
                            {
                                new ActionableInsight
                                {
                                    InsightTitle = "Academic Support Collection",
                                    Description = "Increase technical and professional development titles",
                                    Steps = new List<ImplementationStep>
                                    {
                                        new ImplementationStep
                                        {
                                            StepDescription = "Analyze current academic partnerships",
                                            Priority = 1,
                                            Resources = new List<StepResource>
                                            {
                                                new StepResource
                                                {
                                                    ResourceType = "Database",
                                                    Name = "Academic Institution Contacts",
                                                    AccessMethods = new List<string> { "CRM System", "Partner Portal" }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            UsagePatterns = new List<UsagePattern>
            {
                new UsagePattern
                {
                    PatternType = "Peak Hours Analysis",
                    Metrics = new PatternMetrics
                    {
                        Values = new Dictionary<string, double>
                        {
                            ["MorningPeak"] = 0.85,
                            ["LunchPeak"] = 0.62,
                            ["EveningPeak"] = 0.91
                        },
                        Trends = new List<MetricTrend>
                        {
                            new MetricTrend
                            {
                                MetricName = "Evening Usage",
                                TrendDirection = "Increasing",
                                Magnitude = 0.12,
                                Drivers = new List<string> { "Extended hours", "Digital services", "Working professional schedules" }
                            }
                        }
                    },
                    Components = new List<PatternComponent>
                    {
                        new PatternComponent
                        {
                            ComponentName = "Digital Access",
                            Properties = new Dictionary<string, object>
                            {
                                ["AvailabilityWindow"] = "24/7",
                                ["PeakUtilization"] = "20:00-22:00",
                                ["UserSegment"] = "Working Professionals"
                            },
                            Relationships = new List<ComponentRelationship>
                            {
                                new ComponentRelationship
                                {
                                    RelationshipType = "Correlation",
                                    TargetComponent = "Physical Visits",
                                    Strength = -0.45,
                                    Mechanisms = new List<string> { "Convenience preference", "Time constraints" }
                                }
                            }
                        }
                    },
                    Implications = new List<PatternImplication>
                    {
                        new PatternImplication
                        {
                            ImplicationType = "Operational",
                            Description = "Evening digital services require enhanced technical support",
                            Impacts = new List<BusinessImpact>
                            {
                                new BusinessImpact
                                {
                                    ImpactArea = "Staffing",
                                    Magnitude = 0.25,
                                    Consequences = new List<string> { "Extended IT support hours", "Remote monitoring systems" }
                                }
                            }
                        }
                    }
                }
            },
            Membership = new MembershipAnalytics
            {
                Metrics = new MembershipMetrics
                {
                    TotalMembers = 8450,
                    NewMembers = 234,
                    Growth = new List<MembershipGrowth>
                    {
                        new MembershipGrowth
                        {
                            Period = DateTime.Parse("2024-01-01"),
                            NewRegistrations = 156,
                            Cancellations = 23,
                            Factors = new List<GrowthFactor>
                            {
                                new GrowthFactor
                                {
                                    FactorType = "Digital Outreach",
                                    Impact = 0.68,
                                    Details = new List<string> { "Social media campaigns", "Online registration simplification" }
                                }
                            }
                        }
                    },
                    Distribution = new MembershipDistribution
                    {
                        ByType = new Dictionary<string, int>
                        {
                            ["Standard"] = 6200,
                            ["Student"] = 1850,
                            ["Senior"] = 400
                        },
                        Insights = new List<DistributionInsight>
                        {
                            new DistributionInsight
                            {
                                InsightType = "Demographic Shift",
                                Finding = "Growing student population driving usage patterns",
                                Recommendations = new List<string> { "Expand study spaces", "Increase academic resources" }
                            }
                        }
                    }
                },
                Segments = new List<MemberSegment>
                {
                    new MemberSegment
                    {
                        SegmentName = "Heavy Users",
                        MemberCount = 850,
                        Characteristics = new List<SegmentCharacteristic>
                        {
                            new SegmentCharacteristic
                            {
                                CharacteristicType = "Usage Frequency",
                                Attributes = new Dictionary<string, object>
                                {
                                    ["LoansPerMonth"] = 8.5,
                                    ["VisitsPerMonth"] = 12.3,
                                    ["DigitalEngagement"] = 0.78
                                },
                                Behaviors = new List<string> { "Early technology adopters", "Community event participants", "Feedback providers" }
                            }
                        },
                        Value = new SegmentValue
                        {
                            LifetimeValue = 2850.0,
                            Components = new List<ValueComponent>
                            {
                                new ValueComponent
                                {
                                    ComponentName = "Direct Usage",
                                    Value = 1200.0,
                                    Drivers = new List<string> { "High loan frequency", "Premium services" }
                                },
                                new ValueComponent
                                {
                                    ComponentName = "Community Impact",
                                    Value = 1650.0,
                                    Drivers = new List<string> { "Word of mouth", "Event participation", "Volunteer activities" }
                                }
                            }
                        }
                    }
                },
                Retention = new RetentionAnalysis
                {
                    RetentionRate = 0.89,
                    Cohorts = new List<RetentionCohort>
                    {
                        new RetentionCohort
                        {
                            CohortName = "2023 New Members",
                            StartDate = DateTime.Parse("2023-01-01"),
                            Metrics = new List<CohortMetric>
                            {
                                new CohortMetric
                                {
                                    MonthsAfterStart = 12,
                                    RetentionRate = 0.76,
                                    Observations = new List<string> { "Strong retention for student cohort", "Digital natives show higher engagement" }
                                }
                            }
                        }
                    },
                    ChurnRisks = new List<ChurnRisk>
                    {
                        new ChurnRisk
                        {
                            Member = new Member { Id = 2455, FirstName = "John", LastName = "Smith", Email = "john.smith@email.com" },
                            RiskScore = 0.72,
                            RiskFactors = new List<RiskFactor>
                            {
                                new RiskFactor
                                {
                                    FactorName = "Declining Usage",
                                    Weight = 0.45,
                                    Indicators = new List<string> { "No loans in 60 days", "Reduced digital engagement" }
                                }
                            }
                        }
                    }
                },
                BehaviorPatterns = new List<MemberBehaviorPattern>
                {
                    new MemberBehaviorPattern
                    {
                        PatternName = "Weekend Warriors",
                        Metrics = new List<BehaviorMetric>
                        {
                            new BehaviorMetric
                            {
                                MetricName = "Weekend Activity Ratio",
                                Value = 0.73,
                                Context = new List<MetricContext>
                                {
                                    new MetricContext
                                    {
                                        ContextType = "Demographic",
                                        Data = new Dictionary<string, object>
                                        {
                                            ["PrimaryAge"] = "35-50",
                                            ["EmploymentStatus"] = "Full-time",
                                            ["FamilyStatus"] = "Parents"
                                        }
                                    }
                                }
                            }
                        },
                        Significance = new PatternSignificance
                        {
                            StatisticalSignificance = 0.95,
                            Tests = new List<SignificanceTest>
                            {
                                new SignificanceTest
                                {
                                    TestName = "Chi-Square Test",
                                    PValue = 0.003,
                                    Assumptions = new List<string> { "Independent observations", "Expected frequency > 5" }
                                }
                            }
                        }
                    }
                }
            },
            KPIs = new List<PerformanceIndicator>
            {
                new PerformanceIndicator
                {
                    KPIName = "Collection Turnover Rate",
                    CurrentValue = 3.2,
                    TargetValue = 3.5,
                    Components = new List<KPIComponent>
                    {
                        new KPIComponent
                        {
                            ComponentName = "Fiction Turnover",
                            Weight = 0.4,
                            ContributionValue = 3.8,
                            Influences = new List<string> { "Popular genres", "New releases", "Reading group selections" }
                        }
                    },
                    Status = new KPIStatus
                    {
                        Status = "Below Target",
                        Indicators = new List<StatusIndicator>
                        {
                            new StatusIndicator
                            {
                                IndicatorType = "Trend",
                                Value = "Improving",
                                Actions = new List<string> { "Monitor monthly progress", "Identify underperforming categories" }
                            }
                        }
                    }
                }
            }
        },
        BranchAnalytics = new List<BranchPerformanceAnalysis>
        {
            new BranchPerformanceAnalysis
            {
                Branch = new LibraryBranch { Id = 1, Name = "Main Library", Address = "123 Main St", Phone = "555-0123" },
                Metrics = new BranchMetrics
                {
                    OperationalMetrics = new Dictionary<string, double>
                    {
                        ["DailyVisitors"] = 425.0,
                        ["ServiceRequests"] = 156.0,
                        ["CollectionUtilization"] = 0.72
                    },
                    Efficiency = new List<EfficiencyMeasure>
                    {
                        new EfficiencyMeasure
                        {
                            MeasureName = "Transaction Processing Time",
                            Value = 2.3, // minutes
                            Drivers = new List<EfficiencyDriver>
                            {
                                new EfficiencyDriver
                                {
                                    DriverName = "Staff Training",
                                    Impact = 0.35,
                                    Mechanisms = new List<string> { "Standardized procedures", "Technology proficiency" }
                                }
                            }
                        }
                    },
                    Resources = new ResourceUtilization
                    {
                        Resources = new List<ResourceMetric>
                        {
                            new ResourceMetric
                            {
                                ResourceType = "Study Spaces",
                                UtilizationRate = 0.83,
                                Periods = new List<UtilizationPeriod>
                                {
                                    new UtilizationPeriod
                                    {
                                        Period = "Peak Hours (2-6 PM)",
                                        Rate = 0.95,
                                        Factors = new List<string> { "Student schedules", "Quiet study demand" }
                                    }
                                }
                            }
                        },
                        Summary = new UtilizationSummary
                        {
                            OverallUtilization = 0.78,
                            Insights = new List<UtilizationInsight>
                            {
                                new UtilizationInsight
                                {
                                    InsightType = "Capacity Planning",
                                    Description = "Study spaces at capacity during peak hours",
                                    Recommendations = new List<string> { "Consider reservation system", "Expand evening hours" }
                                }
                            }
                        }
                    }
                },
                Services = new List<ServiceAnalysis>
                {
                    new ServiceAnalysis
                    {
                        ServiceName = "Digital Reference",
                        Metrics = new ServiceMetrics
                        {
                            Values = new Dictionary<string, double>
                            {
                                ["RequestsPerDay"] = 45.0,
                                ["ResponseTime"] = 18.5, // minutes
                                ["SatisfactionScore"] = 4.2
                            },
                            Benchmarks = new List<ServiceBenchmark>
                            {
                                new ServiceBenchmark
                                {
                                    BenchmarkType = "Industry Standard",
                                    BenchmarkValue = 20.0,
                                    CurrentValue = 18.5,
                                    ComparisonNotes = new List<string> { "Exceeding industry standard", "Consistent improvement trend" }
                                }
                            }
                        },
                        Trends = new List<ServiceTrend>
                        {
                            new ServiceTrend
                            {
                                TrendName = "Response Time",
                                DataPoints = new List<ServiceDataPoint>
                                {
                                    new ServiceDataPoint
                                    {
                                        Date = DateTime.UtcNow.AddDays(-30),
                                        Value = 22.1,
                                        Context = new List<string> { "New staff training period", "System upgrades" }
                                    }
                                }
                            }
                        }
                    }
                },
                Comparisons = new List<BranchComparison>
                {
                    new BranchComparison
                    {
                        ComparisonType = "Regional Benchmarking",
                        CompareBranches = new List<LibraryBranch>
                        {
                            new LibraryBranch { Id = 2, Name = "Downtown Branch", Address = "456 Oak St" },
                            new LibraryBranch { Id = 3, Name = "Eastside Branch", Address = "789 Pine St" }
                        },
                        Metrics = new List<ComparisonMetric>
                        {
                            new ComparisonMetric
                            {
                                MetricName = "Visitor Satisfaction",
                                BranchValues = new Dictionary<string, double>
                                {
                                    ["Main Library"] = 4.3,
                                    ["Downtown Branch"] = 4.1,
                                    ["Eastside Branch"] = 4.0
                                },
                                Insights = new List<ComparisonInsight>
                                {
                                    new ComparisonInsight
                                    {
                                        InsightType = "Performance Leader",
                                        Finding = "Main Library leads in visitor satisfaction",
                                        Implications = new List<string> { "Best practices can be shared", "Model for expansion planning" }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },
        CollectionAnalysis = new List<CollectionInsight>
        {
            new CollectionInsight
            {
                CollectionType = "Digital Resources",
                Analysis = new CollectionAnalysisData
                {
                    TotalItems = 125000,
                    Categories = new List<CategoryBreakdown>
                    {
                        new CategoryBreakdown
                        {
                            Category = "E-books",
                            ItemCount = 85000,
                            Metrics = new List<CategoryMetric>
                            {
                                new CategoryMetric
                                {
                                    MetricName = "Usage Rate",
                                    Value = 0.68,
                                    Observations = new List<string> { "Strong mobile adoption", "Popular among commuters" }
                                }
                            }
                        }
                    },
                    Health = new CollectionHealth
                    {
                        HealthScore = 0.87,
                        Indicators = new List<HealthIndicator>
                        {
                            new HealthIndicator
                            {
                                IndicatorName = "Content Freshness",
                                Status = "Good",
                                Factors = new List<string> { "Regular publisher updates", "User-driven acquisitions" }
                            }
                        }
                    }
                },
                Trends = new List<CollectionTrend>
                {
                    new CollectionTrend
                    {
                        TrendType = "Genre Popularity",
                        Points = new List<TrendPoint>
                        {
                            new TrendPoint
                            {
                                Date = DateTime.UtcNow.AddDays(-30),
                                Value = 0.72,
                                Notes = new List<string> { "Mystery/thriller surge", "Seasonal reading patterns" }
                            }
                        }
                    }
                },
                Recommendations = new List<CollectionRecommendation>
                {
                    new CollectionRecommendation
                    {
                        RecommendationType = "Acquisition Strategy",
                        Description = "Expand contemporary fiction collection",
                        Actions = new List<RecommendationAction>
                        {
                            new RecommendationAction
                            {
                                ActionType = "Purchase Orders",
                                Description = "Focus on award-winning recent releases",
                                Priority = 1,
                                Requirements = new List<string> { "Budget allocation", "Publisher negotiations" }
                            }
                        }
                    }
                }
            }
        },
        CustomKPIs = new Dictionary<string, double>
        {
            ["CommunityEngagement"] = 0.78,
            ["DigitalAdoption"] = 0.65,
            ["ResourceEfficiency"] = 0.82
        },
        ReportingData = new AdvancedReportingData
        {
            Reports = new List<ReportMetadata>
            {
                new ReportMetadata
                {
                    ReportName = "Quarterly Performance Dashboard",
                    GeneratedAt = DateTime.UtcNow,
                    Sections = new List<ReportSection>
                    {
                        new ReportSection
                        {
                            SectionName = "Executive Summary",
                            Content = new List<SectionContent>
                            {
                                new SectionContent
                                {
                                    ContentType = "Key Metrics",
                                    Data = new Dictionary<string, object>
                                    {
                                        ["CirculationGrowth"] = 0.12,
                                        ["MemberSatisfaction"] = 4.3,
                                        ["DigitalAdoption"] = 0.65
                                    },
                                    Annotations = new List<string> { "Exceeding targets", "Digital transformation success" }
                                }
                            },
                            Summary = new SectionSummary
                            {
                                KeyFindings = new List<KeyFinding>
                                {
                                    new KeyFinding
                                    {
                                        FindingType = "Growth",
                                        Description = "Digital services driving overall circulation growth",
                                        Impact = 0.78,
                                        SupportingEvidence = new List<string> { "Usage analytics", "Member feedback surveys" }
                                    }
                                },
                                Metrics = new Dictionary<string, double>
                                {
                                    ["OverallPerformance"] = 0.85,
                                    ["TrendMomentum"] = 0.72
                                }
                            }
                        }
                    },
                    Configuration = new ReportConfiguration
                    {
                        Parameters = new Dictionary<string, object>
                        {
                            ["ReportingPeriod"] = "Q1-2024",
                            ["IncludePredictions"] = true,
                            ["DetailLevel"] = "Comprehensive"
                        },
                        Options = new List<ConfigurationOption>
                        {
                            new ConfigurationOption
                            {
                                OptionName = "Output Format",
                                Value = "Interactive Dashboard",
                                Alternatives = new List<string> { "PDF Report", "Excel Workbook", "PowerBI Integration" }
                            }
                        }
                    }
                }
            },
            DataQuality = new DataQualityMetrics
            {
                OverallQuality = 0.91,
                Dimensions = new List<QualityDimension>
                {
                    new QualityDimension
                    {
                        DimensionName = "Completeness",
                        Score = 0.94,
                        Measures = new List<QualityMeasure>
                        {
                            new QualityMeasure
                            {
                                MeasureName = "Missing Values Rate",
                                Value = 0.02,
                                Criteria = new List<string> { "Critical fields < 1%", "Optional fields < 5%" }
                            }
                        }
                    }
                },
                Issues = new List<QualityIssue>
                {
                    new QualityIssue
                    {
                        IssueType = "Data Consistency",
                        Severity = "Low",
                        Description = "Minor format inconsistencies in legacy records",
                        Resolution = new List<ResolutionStep>
                        {
                            new ResolutionStep
                            {
                                StepDescription = "Implement automated data cleaning",
                                ResponsibleRole = "Data Manager",
                                Resources = new List<string> { "ETL Tools", "Validation Scripts" }
                            }
                        }
                    }
                }
            },
            ExportOptions = new List<ExportOption>
            {
                new ExportOption
                {
                    ExportType = "Analytics Dashboard",
                    Formats = new List<ExportFormat>
                    {
                        new ExportFormat
                        {
                            FormatName = "Power BI",
                            Options = new List<FormatOption>
                            {
                                new FormatOption
                                {
                                    OptionName = "Refresh Frequency",
                                    DefaultValue = "Daily",
                                    PossibleValues = new List<string> { "Real-time", "Hourly", "Daily", "Weekly" }
                                }
                            }
                        }
                    },
                    Configuration = new ExportConfiguration
                    {
                        Settings = new Dictionary<string, object>
                        {
                            ["IncludeHistorical"] = true,
                            ["MaxRecords"] = 1000000,
                            ["CompressionLevel"] = "High"
                        },
                        Filters = new List<ExportFilter>
                        {
                            new ExportFilter
                            {
                                FilterType = "Date Range",
                                Criteria = new Dictionary<string, object>
                                {
                                    ["StartDate"] = DateTime.UtcNow.AddMonths(-12),
                                    ["EndDate"] = DateTime.UtcNow
                                }
                            }
                        }
                    }
                }
            }
        }
    };
});

// Collection Analysis endpoint with deeply nested structures
app.MapPost("/api/analytics/collections/detailed-analysis", (List<string> collectionTypes) =>
{
    return collectionTypes.Select(type => new CollectionInsight
    {
        CollectionType = type,
        Analysis = new CollectionAnalysisData
        {
            TotalItems = Random.Shared.Next(50000, 200000),
            Categories = Enumerable.Range(1, 5).Select(i => new CategoryBreakdown
            {
                Category = $"{type} Category {i}",
                ItemCount = Random.Shared.Next(5000, 25000),
                Metrics = new List<CategoryMetric>
                {
                    new CategoryMetric
                    {
                        MetricName = "Popularity Index",
                        Value = Random.Shared.NextDouble(),
                        Observations = new List<string> { "Trending upward", "User engagement high" }
                    }
                }
            }).ToList(),
            Health = new CollectionHealth
            {
                HealthScore = Random.Shared.NextDouble(),
                Indicators = new List<HealthIndicator>
                {
                    new HealthIndicator
                    {
                        IndicatorName = "Content Relevance",
                        Status = "Excellent",
                        Factors = new List<string> { "Regular updates", "Community feedback integration" }
                    }
                }
            }
        },
        Trends = new List<CollectionTrend>
        {
            new CollectionTrend
            {
                TrendType = "Usage Growth",
                Points = Enumerable.Range(1, 12).Select(month => new TrendPoint
                {
                    Date = DateTime.UtcNow.AddMonths(-month),
                    Value = Random.Shared.NextDouble() * 100,
                    Notes = new List<string> { $"Month {month} observations", "Seasonal patterns noted" }
                }).ToList()
            }
        },
        Recommendations = new List<CollectionRecommendation>
        {
            new CollectionRecommendation
            {
                RecommendationType = "Strategic Enhancement",
                Description = $"Optimize {type} collection based on usage patterns",
                Actions = new List<RecommendationAction>
                {
                    new RecommendationAction
                    {
                        ActionType = "Content Acquisition",
                        Description = "Expand high-demand subcategories",
                        Priority = 1,
                        Requirements = new List<string> { "Budget approval", "Vendor negotiations", "Space allocation" }
                    }
                }
            }
        }
    });
});

// Branch Comparison Analysis endpoint
app.MapGet("/api/analytics/branches/performance-comparison", () =>
{
    return new List<BranchComparison>
    {
        new BranchComparison
        {
            ComparisonType = "System-wide Performance",
            CompareBranches = new List<LibraryBranch>
            {
                new LibraryBranch { Id = 1, Name = "Main Library", Address = "123 Main St" },
                new LibraryBranch { Id = 2, Name = "Downtown Branch", Address = "456 Oak St" },
                new LibraryBranch { Id = 3, Name = "Suburban Branch", Address = "789 Pine Ave" }
            },
            Metrics = new List<ComparisonMetric>
            {
                new ComparisonMetric
                {
                    MetricName = "Operational Efficiency",
                    BranchValues = new Dictionary<string, double>
                    {
                        ["Main Library"] = 0.87,
                        ["Downtown Branch"] = 0.82,
                        ["Suburban Branch"] = 0.91
                    },
                    Insights = new List<ComparisonInsight>
                    {
                        new ComparisonInsight
                        {
                            InsightType = "Best Practice Identification",
                            Finding = "Suburban branch leads in efficiency metrics",
                            Implications = new List<string> 
                            { 
                                "Standardize successful processes across branches",
                                "Investigate suburban branch workflow innovations",
                                "Consider resource reallocation opportunities"
                            }
                        }
                    }
                }
            }
        }
    };
});

app.Run();