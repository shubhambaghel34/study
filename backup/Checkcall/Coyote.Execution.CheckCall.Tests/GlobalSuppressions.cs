// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright © 2016 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom", Scope = "member", Target = "Coyote.Execution.CheckCall.Tests.Unit.SendDailyCheckCallEmailHandlerTests.#TestInit()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Cancelled", Scope = "member", Target = "Coyote.Execution.CheckCall.Tests.Unit.Saga.DailyCheckCallSagaTests.#DailyCheckCallSaga_SagaCompleteWhenLoadCancelled()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Cancelled", Scope = "member", Target = "Coyote.Execution.CheckCall.Tests.Unit.Saga.DailyCheckCallSagaTests.#DailyCheckCallSaga_SagaNotCompleteWhenLoadCancelled_MultipleLoads()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "LTL", Scope = "member", Target = "Coyote.Execution.CheckCall.Tests.Unit.Processor.CheckCallLoadBookedEventProcessorTests.#LoadBookedEventProcessor_LTL_NoSagaEventPublished()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Coyote.Execution.CheckCall.Tests.Integration.Support.Randoms.#Password")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Coyote.Execution.CheckCall.Tests.Integration.DailyCheckCallTestContext.#MessagesSent")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "Coyote.Execution.CheckCall.Tests.Integration.EndpointBuilders.DailyCheckCall_IntegrationTest_Email_Endpoint+SendDailyCheckCallInspector.#MutateIncoming(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Coyote.Execution.CheckCall.Tests.Integration.EndpointBuilders.DailyCheckCall_IntegrationTest_Email_Endpoint+SendDailyCheckCallInspector")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "member", Target = "Coyote.Execution.CheckCall.Tests.Integration.DailyCheckCallTests.#DailyCheckCall_OnlyOneLoadForTestCarrier()")]

