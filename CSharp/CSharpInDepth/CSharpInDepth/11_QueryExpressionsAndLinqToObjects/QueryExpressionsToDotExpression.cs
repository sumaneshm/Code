using CSharpInDepth.Common;
using Model;
using System;
using System.Linq;

namespace CSharpInDepth._11_QueryExpressionsAndLinqToObjects
{
	internal class QueryExpressionsToDotExpression : Study
	{
		public override string StudyName
		{
			get { return "Analysis of how the compiler translates Linq Query expressions to dot expressions"; }
		}

		protected override void PerformStudy()
		{
			// !!!! All the query expressions gets transformed to dot notation as a precompiler step !!!!

			// The complete query is deferred execution that is, unless and until anyone ask for the next item, it wont execute proceed further

			//SimpleSelectQuery();
			//ReturnAnonymousType();
			//UseSimpleLetClause();
			//UseMultipleLetClause();
			//InnerJoin();
			//GroupJoin();
			//CrossJoin();
			//GroupBy();

			QueryContinuation();
		}

		private void QueryContinuation()
		{
			var query = from defect in SampleData.AllDefects
						where defect.AssignedTo != null
						group defect by defect.AssignedTo into grouped
						select new { AssignedTo = grouped.Key, Count = grouped.Count() } into result
						orderby result.Count descending
						select result;

			foreach (var oneGroup in query)
			{
				Console.WriteLine("{0}-{1}", oneGroup.AssignedTo, oneGroup.Count);
			}
		}

		private void GroupBy()
		{
			var query = from defect in SampleData.AllDefects
						where defect.AssignedTo != null
						group defect by defect.AssignedTo;

			foreach (var oneGroup in query)
			{
				Console.WriteLine(oneGroup.Key);
				foreach (var oneDefect in oneGroup)
				{
					Console.WriteLine("\t" + oneDefect.Summary);
				}
			}
		}

		private void CrossJoin()
		{

			DrawHeader("Cross join is used when we dont have to map any fields, but just need a cartisean product of all the elements from both the joined tables");

			var query = from defect in SampleData.AllDefects
						from subscriber in SampleData.AllSubscriptions
						select new { defect.Summary, subscriber.EmailAddress };

			foreach (var oneEntry in query)
			{
				Console.WriteLine("{0}-{1}", oneEntry.Summary, oneEntry.EmailAddress);
			}

			/*
			 IEnumerable<TOutput> Enumerable.SelectMany
									(this IEnumerable<TOuter> outer,
									 Func<TOuter, IEnumerable<TInner>)
									 Func<TInner, TOutput, TInner> 
									 );
	
			 */

			// 1. This statement is completely streamed
			// 2. 
		}

		private void GroupJoin()
		{
			DrawHeader("Gets a list of subscribers grouped by defect summary ");

			var query = from defect in SampleData.AllDefects
						join subscriber in SampleData.AllSubscriptions
							on defect.Project equals subscriber.Project
							into groupedSubscriptions
						select new { defect.Summary, GroupedSubscriptions = groupedSubscriptions };

			foreach (var oneItem in query)
			{
				Console.WriteLine(oneItem.Summary);
				foreach (var sub in oneItem.GroupedSubscriptions)
				{
					Console.WriteLine("\t{0}", sub.EmailAddress);
				}
			}

			//Translated dotnet quotation

			/*
			 IEnumerable<TOutput> Enumerable.GroupJoin
									(this IEnumerable<T1> outer,
									 IEnumerable<T2> inner,
									 Func<T1, TKey> outerKeySelector,
									 Func<T2, TKey> innerKeySelector,
									 Func<T1, IEnumerable<T2>, TOuter> resultSelector);
			 */
		}

		private void InnerJoin()
		{
			DrawHeader("Get a list of defect's Summary and the user who has subscribed for the same");

			var query = from subscriber in SampleData.AllSubscriptions
						join defect in SampleData.AllDefects
						on subscriber.Project equals defect.Project
						select new { defect.Summary, subscriber.EmailAddress };

			foreach (var info in query)
			{
				Console.WriteLine("{0}-{1}", info.EmailAddress, info.Summary);
			}

			//Salient points to remember about the above simple inner join
			// 1. It will list one element from the first and ALL the items from the right
			// 2. CLR will cache the "right" element and Streams through all the elements in the left 
			//      Tip: If we want to join massive sequence with a tiny one, it is good to use the tiny one on the right side
			// 3. We have to always use the left side (used in the from statement) on the left side of "equals" and whatever on the right side to the right side of "equals"

			// It gets transferred to the following dot notation
			/*
			 IEnumerable<T3> Enumerable.Join(
								 this IEnumerable<T1> outer,
								 IEnumerable<T2> inner,
								 Func<T1, T3> outerKeySelector,
								 Func<T2, T3> innerKeySelector,
								 Func<T1,T2,T3> resultSelector
								);
	
			 */

		}

		private void ReturnAnonymousType()
		{
			//The following query just creates an anonymous type and returns it back
			var query = from user in SampleData.AllUsers
						select new { user.Name, Length = user.Name.Length };

			foreach (var userInfo in query)
			{
				Console.WriteLine("{0}-{1}", userInfo.Name, userInfo.Length);
			}
		}

		private void SimpleSelectQuery()
		{
			/*
			 * The following query gets translated to use .Select(TInput,
			 */
			//var query = from user in SampleData.AllUsers select user.Name;
			var query = SampleData.AllUsers.Select(u => u.UserType);

			foreach (var user in query)
			{
				Console.WriteLine(user);
			}
		}
		private void UseMultipleLetClause()
		{
			//First let creates a new anonymous type which holds "user" and "userNameLength" and returns to outer loop
			//and outer loop returns the "previous loop's" anonymous type and includes "userName" and returns a new anonymous type
			var query = from user in SampleData.AllUsers
						let userName = user.Name
						let userNameLength = user.Name.Length
						select string.Format("{0}-{1}", userName, userNameLength);

			foreach (var userInfo in query)
			{
				Console.WriteLine(userInfo);
			}
		}

		private void UseSimpleLetClause()
		{
			//The following creates an anonymous type to hold both "user" and new variable called "userNameLength" and returns to the next enumerable
			// which inturn can make use of both and finally it only projects userNameLength.

			//The current version of C# compiler is not that intelligent that it fails to recognize that we are NOT using the user in subsequent queries and hence can
			//directly return the user.Name.Length instead of going a round about way to create a new anonymous method.

			// Perhaps a scope for future development !!!

			DrawHeader("The new anonymous type which is created due to the use of let keyword is called as 'Transparent identifier'");

			var query = from user in SampleData.AllUsers
						let userNameLength = user.Name.Length
						select userNameLength;

			foreach (var userInfo in query)
			{
				Console.WriteLine(userInfo);
			}
		}
	}
}