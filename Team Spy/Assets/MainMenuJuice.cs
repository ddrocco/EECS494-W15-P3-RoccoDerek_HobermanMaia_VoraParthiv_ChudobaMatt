using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenuJuice : MonoBehaviour {
	public List<string> snippets = new List<string>();
	public GameObject JuiceText;
	Canvas canv;
	float timer = 3;
	float timeBetweenSpawns = 0.75f;
	
	void Start() {
		canv = GetComponent<Canvas>();
		snippets.Add ("data.clear();\nfor (int i = 0; start != end; start++, i++)\n\tdata[i] = *start;\ncompare = comp;");
		snippets.Add ("data.clear();\ncompare = comp;");
		snippets.Add ("int opt = 0, index = 0;\ndata.verbose = false;\ndata.median = false;\ndata.summary = false;\ndata.transfers = false;\n");
		snippets.Add ("switch(opt)\n{\n\tcase 's':\n\t\tdata.summary = true;\n\t\tbreak;\n\tcase 'v':\n\t\tdata.verbose = true;\n\t\tbreak;\n\tcase 'm':\n\t\tdata.median = true;\n\t\tbreak;\n\tcase 't':\n\t\tdata.transfers = true;\n\t\tbreak;\n\tcase 'i':\n\t\tdata.i.push_back(optarg);\n\t\tbreak;\n\tcase 'g':\n\t\tdata.g.push_back(optarg);\n\tbreak;");
		snippets.Add ("while (true) {\n\torder* a = new order;\n\tstring buy_or_sell;\n\tchar usd;\n\tchar lb;\n\tcin >> a->timestamp >> a->client >> buy_or_sell >> a->symbol\n\t\t>> usd >> a->price >> lb >> a->quantity;\n\tif (cin.eof())\n\t\tbreak;\n\tif (a->timestamp < 0) exit(1);\n\tif (a->price <= 0) exit(1);\n\tif (a->timestamp < last_timestamp) exit(1);\n\tlast_timestamp = a->timestamp;\n\tif (usd != '$' || lb != '#') exit(1);\n\tif (a->symbol.length() == 0 || a->symbol.length() > 5) exit(1);\n\tif (!valid_client(a->client)) exit(1);\n\tif (strcmp(buy_or_sell.c_str(),\"BUY\") == 0) a->buy = true;\n\telse if (strcmp(buy_or_sell.c_str(),\"SELL\") == 0) a->buy = false;\n\telse exit(1);\n\ta->ID = identification++;\n\torders.push_back(a);\n}");
		snippets.Add ("//INSIDER:\nfor (unsigned int i = 0; i < args.i.size(); ++i)\n{\n\tmarket.insert(make_pair(args.i[i],empty_stock));\n\tmarket[args.i[i]].insider = true;\n\tstring insider = \"INSIDER_\" + args.i[i];\n\tnet.insert(make_pair(insider,empty_transfer));\n}\n\n//TIME TRAVELER:\nfor (unsigned int i = 0; i < args.g.size(); ++i)\n{\n\tmarket.insert(make_pair(args.g[i],empty_stock));\n\tmarket[args.g[i]].tt = true;\n}");
		snippets.Add ("market[subject->symbol].push_back(subject);\nif (!subject->quantity) {\n\tmarket[subject->symbol].pop(subject->buy);\n}");
		snippets.Add ("cout << \"---End of Day---\\n\";\nif (args.summary)   //Summary\n\tcout << \"Commission Earnings: $\" << comm << \"\\n\"\n\t<< \"Total Amount of Money Transferred: $\" << transferred << \"\\n\"\n\t<< \"Number of Completed Trades: \" << trades << \"\\n\\\"\n\t<< \"Number of Shares Traded: \" << shares << \"\\n\";\nif (args.transfers)  //Transfers {\n\tfor (auto j = net.begin(); j != net.end(); ++j) {\n\t\tcout << j->first << \" bought \" << j->second.bought << \" and sold \"\n\t\t<< j->second.sold << \" for a net transfer of $\" << j->second.profit\n\t\t<< \"\\n\";\n\t}\n}\nfor (unsigned int i = 0; i < args.g.size(); ++i) {\n\tstock* tt = &market[args.g[i]];\n\tif (tt->is_valid())\n\t\tcout << \"Time travelers would buy \" << args.g[i] << \" at time: \"\n\t\t<< tt->sale << \" and sell it at time: \" << tt->purchase << \"\\n\";\n\telse\n\t\tcout << \"Time travelers would buy \" << args.g[i] << \" at time: -1\"\n\t\t<< \" and sell it at time: -1\\n\";\n}\nfor (auto i = orders.begin(); i != orders.end(); ++i)\n\tdelete *i;\nreturn 0;");
		snippets.Add ("if (outputMode == 'M') {\n\tif (finish != 2147483647)\n\t\ttrace(array, finish, N, area);\n//else: NO BUCK FOUND; no need to retrace path; just go on to print\nfor (long unsigned int i = 0; i < size; ++i) {\n\t\tif (i % N == 0)	{\n\t\tss << \"\\n\";\n\t\tif (i % area == 0)\n\t\t\tss << \"//farm \" << i/area << \"\\n\";\n\t\t}\n\t\tss << array[i].type;\n\t}\n\tss << \"\\n\";\n}");
		snippets.Add ("while (true) {\n\tchar i = array[start].type;\n\tif (i == 'B')\n\t\tbreak;\n\tss << \"\\n(\" << (start % area) / N << \",\" << start % N\n\t\t\t<< \",\" << start / area << \",\" << i << \")\";\n\tif (i == 'n')   //Backtrack North\n\t\tstart -= N;\n\telse if (i == 'w')   //Backtrack East\n\t\tstart -= 1;\n\telse if (i == 's')   //Backtrack South\n\t\tstart += N;\n\telse if (i == 'e')   //Backtrack West\n\t\tstart += 1;\n\telse if (i == 'd')   //Backtrack Up\n\t\tstart -= area;\n\telse if (i == 'u')   //Backtrack Down\n\t\tstart += area;\n}");
		snippets.Add ("//NORTH\nif ((index % area) / N != 0)  //Makes sure not at northern border\n\tif (array[index - N].parent == 'f') //Makes sure unvisited\n\t{\n\t\tarray[index - N].parent = 'n';\n\t\tpath.push_back(index - N);\n\t\tif (array[index - N].type == 'B')\n\t\t\tbreak;\n\t}\n//EAST\nif ((index % N) != N - 1) //Makes sure not at eastern border\n\tif (array[index + 1].parent == 'f') //Makes sure unvisited\n\t{\n\t\tarray[index + 1].parent = 'e';\n\t\tpath.push_back(index + 1);\n\t\tif (array[index + 1].type == 'B')\n\t\t\tbreak;\n\t}\n//SOUTH\nif (((index % area) / N) != N - 1) //Makes sure not southern border\n\tif (array[index + N].parent == 'f') //Makes sure unvisited\n\t{\n\t\tarray[index + N].parent = 's';\n\t\tpath.push_back(index + N);\n\t\tif (array[index + N].type == 'B')\n\t\t\tbreak;\n\t}\n//WEST\nif ((index % N) != 0) //Makes sure not at western border\n\tif (array[index - 1].parent == 'f') //Makes sure unvisited\n\t{\n\t\tarray[index - 1].parent = 'w';\n\t\tpath.push_back(index - 1);\n\t\tif (array[index - 1].type == 'B')\n\t\t\tbreak;\n\t}");
		snippets.Add ("void next()\n{\n\tdouble best_dist = MAX;\n\tint best_loc = 0;\n\tfor (int i = 0; i < locations; ++i)\n\t{\n\t\tif (unvisited[i])\n\t\t{\n\t\t\tdouble curr_dist = dist(current,i);\n\t\t\tif (curr_dist < best_dist)\n\t\t\t{\n\t\t\t\tbest_dist = curr_dist;\n\t\t\t\tbest_loc = i;\n\t\t\t}\n\t\t}\n\t\t}\n\t\troute[counter++] = best_loc;\n\t\tcurrent = best_loc;\n\t\tunvisited[best_loc] = false;\n\t\ttotal += best_dist;\n}");
		snippets.Add ("for (int i = 0; i < locations; ++i)\n{\n\tcin >> master[i].x >> master[i].y;\n\tif (master[i].x < lx || master[i].x > rx || master[i].y < ly || master[i].y > ry)\n\t{\n\t\tmaster[i].out = true;\n\t\tmaster[i].in = false;\n\t\tsol_out = true;\n\t}\n\telse if (master[i].x == lx || master[i].x == rx || master[i].y == ly || master[i].y == ry)\n\t{\n\t\tmaster[i].out = true;\n\t\tmaster[i].in = true;\n\t\tsol_border = true;\n\t}\n\telse\n\t{\n\t\tmaster[i].out = false;\n\t\tmaster[i].in = true;\n\t\tsol_in = true;\n\t}\n\tvisited[i] = false;\n\tbest_distance[i] = MAX;\n}");
		snippets.Add ("low_bound_total = 0;\nfor (int i = 0; i < locations; ++i)\n{\n\tdouble a = MAX;\n\tdouble b = MAX;\n\tfor (int j = 0; j < locations; ++j)\n\t{\n\t\tdouble c;\n\t\tif (i == j) c = MAX;\n\t\telse if (i < j)    c = dist(i,j);\n\t\telse    c = edges[j][i];\n\t\tedges[i][j] = c;\n\t\tif (c < a)\n\t\t{\n\t\t\ta = b;\n\t\t\tb = c;\n\t\t}\n\t\telse if (c < b)  b = c;\n\t}\n\tlow_bound[i] = (a + b) / 2;\n\tlow_bound_total += (a + b) / 2;\n}\nRoute* a = new Route;\nup_bound_total = MAX;\ninitialize(a);\nbranch(a);\ndelete a;");
		snippets.Add ("void branch(Route *a)\n{\n\tif (a->visited == locations)\n\t{\n\t\ta->low_bound += edges[0][a->current] - low_bound[a->current];\n\t\tif (a->low_bound < up_bound_total)\n\t\t{\n\t\t\tup_bound_total = a->low_bound;\n\t\t\troute = a->route;\n\t\t}\n\t\treturn;\n\t}\n\tfor (int j = a->visited; j < locations; ++j)\n\t{\n\t\tint i = a->route[j];\n\t\tdouble curr_dist = edges[i][a->current];\n\t\tdouble new_low_bound = a->low_bound - low_bound[a->current] + curr_dist;\n\t\tif (new_low_bound < up_bound_total)\n\t\t{\n\t\t\tRoute* b = new Route(a);\n\t\t\tb->low_bound = new_low_bound;\n\t\t\tb->current = i;\n\t\t\tswap(b->route[b->visited],b->route[j]);\n\t\t\t++b->visited;\n\t\t\tbranch(b);\n\t\t\tdelete b;\n\t\t\t++q;\n\t\t}\n\t}\n}");
	}
	
	void Update () {
		timer += Time.deltaTime;
		if (timer > timeBetweenSpawns) {
			timer -= timeBetweenSpawns;
			SpawnText();
		}
	}
	
	void SpawnText() {
		Rect rect = canv.pixelRect;
		Vector3 displacement = new Vector3(Random.Range(-rect.width / 2, rect.width / 2), Random.Range(-rect.height / 2, rect.height / 2), 0);
		Vector2 velocity = Random.Range (0, 5f) * Random.insideUnitCircle;
		
		GameObject JuicyText = Instantiate(JuiceText) as GameObject;
		JuicyText.transform.SetParent(parent: transform, worldPositionStays: false);
		JuicyText.GetComponent<RectTransform>().anchoredPosition = rect.center;
		JuicyText.transform.localPosition = displacement;
		JuicyText.transform.localScale = Vector3.one;
		JuicyText.GetComponent<IntroJuiceText>().velocity = new Vector3(velocity.x, velocity.y, 0);
		JuicyText.GetComponent<IntroJuiceText>().baseColor = new Color(0, 0.5f, 0, 0.5f);
		JuicyText.GetComponent<Text>().text = GetRandom (snippets);
	}
	
	string GetRandom(List<string> snippets) {
		float f = Random.Range(0, snippets.Count);
		int i = Mathf.FloorToInt(f);
		if (i == snippets.Count) {
			return snippets[0];
		} else {
			return snippets[i];
		}
	}
}
