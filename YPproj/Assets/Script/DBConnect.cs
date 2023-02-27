using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System.Data;
using System;


public class DBConnect : MonoBehaviour
{



    string db = "host=121.78.158.27;username=postgres;password=egis0700;database=unity";
    void Start()
    {

        /*using (var conn = new NpgsqlConnection("host=localhost;username=postgres;password=admin;database=UnityZombie"))
        {
            try
            {
                conn.Open();
                using(var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "SELECT * FROM \"SCORE\" ORDER BY score DESC";

                    int rank = 1;

                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.Log(reader["id"].ToString());
                            Debug.Log(reader["score"].ToString());
                            GameObject instantiate = Instantiate(Resources.Load("List", typeof(GameObject))) as GameObject;
                            instantiate.transform.SetParent( GameObject.Find("RankContent").transform);

                            Text[] textList = instantiate.GetComponentsInChildren<Text>();
                            textList[0].text = rank.ToString();
                            textList[1].text = reader["id"].ToString();
                            textList[2].text = reader["score"].ToString();

                            rank++;

                        }
                    }
                }
            }
            catch
            {
                Debug.Log("ERROR");
            }
        }*/

        SelectRoomNo();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InsertQuery(string query)
    {
        using (var conn = new NpgsqlConnection(db))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = query;

                    cmd.ExecuteNonQuery();

                    /*using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.Log(reader["id"].ToString());
                            Debug.Log(reader["score"].ToString());
                        }
                    }*/
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex);

            }
        }
    }

    public DataTable selectRank()
    {

        DataTable table = new DataTable();
        table.Columns.Add(new DataColumn("rank", typeof(string)));
        table.Columns.Add(new DataColumn("id", typeof(string)));
        table.Columns.Add(new DataColumn("score", typeof(string)));

        using (var conn = new NpgsqlConnection(db))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "with zom_rank as (" +
                            "select " +
                            "zri.room_no , " +
                            "zri.room_score ," +
                            "zui.usr_id ," +
                            "zri.room_dt " +
                            "from zom_rm_info zri " +
                            "join " +
                            "zom_usr_info zui " +
                            "on " +
                            "zui.room_no = zri.room_no " +
                            "order by room_score desc) " +
                            "select " +
                            "room_no," +
                            "room_score," +
                            "array_to_string(array_agg(usr_id), ', ') user_id," +
                            "to_char(room_dt, 'YYYY-MM-DD') " +
                            "from " +
                            "zom_rank " +
                            "group by room_no, room_score, room_dt " +
                            "order by room_score desc; ";


                    int rank = 1;



                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DataRow row = null;



                            row = table.NewRow();

                            row["rank"] = rank.ToString();
                            row["id"] = reader["user_id"].ToString();
                            row["score"] = reader["room_score"].ToString();

                            table.Rows.Add(row);



                            /*GameObject instantiate = Instantiate(Resources.Load("List", typeof(GameObject))) as GameObject;
                            instantiate.GetComponent<sizeChecker>().setRank(rank, reader["id"].ToString(), int.Parse( reader["score"].ToString()));
                            instantiate.transform.SetParent(rankContent.transform);*/
                            rank++;
                        }
                    }
                }

                return table;
            }
            catch (Exception ex)
            {

                Debug.Log(ex);
            }
        }
        return null;
    }

    public String SelectRoomNo()
    {
        using (var conn = new NpgsqlConnection(db))
        {
            try
            {
                conn.Open();
                Debug.Log("진입");
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = @"select COUNT(*)+1 room_no from zom_rm_info;";


                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Debug.Log("조회성공 :  " + reader["room_no"].ToString() );
                            return reader["room_no"].ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                Debug.Log("에러");
            }
        }
        Debug.Log("끝");
        return null;
    }

}
